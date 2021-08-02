using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelsiScanner
{
    internal class Scanner
    {
        private record SlotInfo(DateTime Date, string Description);

        private const int KyivSettlementId = 1;

        private const string VaccinationSpecialtyId = "0c77f1d6-d7d6-46b8-893e-ac5ff5fb438a";

        private readonly IEnumerable<string> RequiredDoctorTags = new string[]
        {
            "moderna",
            "astra",
            "pfizer",
            //"corona"
        };

        private const int DayRangeFromToday = 14;

        private readonly IHelsiClient _helsiClient;

        public Scanner(IHelsiClient helsiClient)
        {
            _helsiClient = helsiClient;
        }

        public async Task<IEnumerable<string>> ScanAsync()
        {
            var openSlots = new List<SlotInfo>();

            var doctors = await GetMatchingDoctorsAsync();
            await Task.Delay(500);

            foreach (var doctor in doctors)
            {
                var doctorSlots = await GetDoctorOpenSlotsAsync(doctor.ResourceId);
                await Task.Delay(500);

                foreach (var slot in doctorSlots)
                {
                    var slotDescription = $"{doctor.ResourceId} | {doctor.FirstName} | {doctor.LastName} | {doctor.MiddleName}";
                    openSlots.Add(new SlotInfo(slot.StartDate, slotDescription));
                }
            }

            return openSlots
                .OrderBy(x => x.Date)
                .Select(x => $"{x.Description} >>> {x.Date}")
                .Distinct();
        }

        private async Task<IEnumerable<Dtos.Slot>> GetDoctorOpenSlotsAsync(string doctorId)
        {
            var slots = await _helsiClient.GetDoctorSlots(
                doctorId,
                DateTime.Today.ToString("yyyy-MM-dd"),
                DateTime.Today.AddDays(DayRangeFromToday).ToString("yyyy-MM-dd"));

            var openSlots = slots
                .Where(x => x.Status == Dtos.SlotStatus.Open)
                .ToList();

            return openSlots;
        }

        private async Task<IEnumerable<Dtos.Doctor>> GetMatchingDoctorsAsync()
        {
            var doctorPage = await _helsiClient.GetDoctors(KyivSettlementId, VaccinationSpecialtyId);

            var matchingDoctors = doctorPage.Data
                .Where(IsMatchingDoctor)
                .ToList();

            return matchingDoctors;
        }

        private bool IsMatchingDoctor(Dtos.Doctor doctor)
        {
            var description = $"{doctor.FirstName} {doctor.LastName} {doctor.MiddleName}";

            return RequiredDoctorTags.Any(x => description.Contains(x, StringComparison.OrdinalIgnoreCase));
        }
    }
}
