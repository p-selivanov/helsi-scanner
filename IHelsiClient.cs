using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelsiScanner.Dtos;
using Refit;

namespace HelsiScanner
{
    internal interface IHelsiClient
    {
        [Get("/api/healthy/doctors?settlement={settlementId}&specialityId={specialityId}")]
        Task<EntityPage<Doctor>> GetDoctors(int settlementId, string specialityId);

        [Get("/api/v1/doctors/{doctorId}/slots?endDate={endDate}&startDate={startDate}")]
        Task<IEnumerable<Slot>> GetDoctorSlots(string doctorId, string startDate, string endDate);
    }
}
