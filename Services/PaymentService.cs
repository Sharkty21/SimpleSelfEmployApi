using AutoMapper;
using MongoDB.Bson;
using SimpleSelfEmployApi.Data;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(PaymentRepository PaymentRepository, IMapper mapper)
        {
            _repository = PaymentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> Index(int? page, string? jobId, int limit = 10)
        {
            var query = await _repository.Index(page, jobId, limit);
            return query.ToList().Select(j => j.PaymentDto);
        }

        public async Task<PaymentDto> SavePayment(PaymentDto Payment)
        {
            Payment PaymentModel = _mapper.Map<Payment>(Payment);

            if (PaymentModel == null)
            {
                throw new Exception("Unable to map Payment dto.");
            }

            if (PaymentModel.Id == ObjectId.Empty)
            {
                await _repository.InsertOneAsync(PaymentModel);
                return PaymentModel.PaymentDto;
            }

            await _repository.ReplaceOneAsync(PaymentModel ?? new Payment());
            return PaymentModel.PaymentDto;
        }

        public async Task<PaymentDto> GetPayment(string id)
        {
            try
            {
                var Payment = await _repository.FindByIdAsync(id);
                return Payment.PaymentDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeletePayment(string id)
        {
            try
            {
                await _repository.DeleteByIdAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
