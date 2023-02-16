using DLock.Field.WebApi.Entities;
using DLock.Field.WebApi.Persistence;
using DLock.Models;
using Microsoft.EntityFrameworkCore;

namespace DLock.Field.WebApi.Services
{
    public interface IFieldService
    {
        Task<IEnumerable<FieldModel>> GetAllFields();
        Task<FieldModel> FindField(string name);
        Task ChangeMaintenanceStatus(string name, bool underMaintenance);
    }

    public class FieldService : IFieldService
    {
        private readonly FieldContext _dbContext;
        private readonly IConfiguration _configuration;

        public FieldService(FieldContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task ChangeMaintenanceStatus(string name, bool underMaintenance)
        {
            IQueryable<FieldEntity> query = _dbContext.Field;

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name == name);
            }

            var entities = await query.ToArrayAsync();

            foreach (var entity in entities)
            {
                entity.IsUnderMaintenance = underMaintenance;
                entity.MaintenanceTime = underMaintenance ? DateTimeOffset.UtcNow : null;
            }

            await Task.Delay(_configuration.GetValue<int>("DelayTime")); // [IMPORTANT] simulate long running operation

            await _dbContext.SaveChangesAsync();
        }

        public async Task<FieldModel> FindField(string name)
        {
            var entity = await _dbContext.Field.FindAsync(name);

            return new FieldModel
            {
                Name = entity.Name,
                IsUnderMaintenance = entity.IsUnderMaintenance,
                MaintenanceTime = entity.MaintenanceTime,
            };
        }

        public async Task<IEnumerable<FieldModel>> GetAllFields()
        {
            var models = await _dbContext.Field
                .Select(f => new FieldModel
                {
                    Name = f.Name,
                    IsUnderMaintenance = f.IsUnderMaintenance,
                    MaintenanceTime = f.MaintenanceTime,
                })
                .OrderBy(f => f.Name)
                .ToArrayAsync();

            return models;
        }
    }
}
