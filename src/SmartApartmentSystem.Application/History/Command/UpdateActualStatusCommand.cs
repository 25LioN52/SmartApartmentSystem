using MediatR;
using SmartApartmentSystem.Domain.Entity;
using SmartApartmentSystem.Domain.Extensions;
using SmartApartmentSystem.Domain.WaterTemperature;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartmentSystem.Application.History.Command
{
    public class UpdateActualStatusCommand : IRequest<ResultStatus>
    {
        public IReadOnlyDictionary<WaterTempChannels, ModuleStatus> Model { get; set; }
    }

    public class UpdateActualStatusCommandHandler : IRequestHandler<UpdateActualStatusCommand, ResultStatus>
    {
        private readonly ISasDb _sasDb;

        public UpdateActualStatusCommandHandler(ISasDb sasDb)
        {
            _sasDb = sasDb;
        }

        public async Task<ResultStatus> Handle(UpdateActualStatusCommand request, CancellationToken cancellationToken)
        {
            foreach (var status in request.Model)
            {
                _sasDb.ModuleActuals.Add(new ModuleActual
                {
                    ActualStatus = status.Value.ActualStatus,
                    ChangeDate = DateTime.Now,
                    IsActive = status.Value.IsActive,
                    ModuleId = status.Key.ToGlobalType()
                });
            }
            await _sasDb.SaveChangesAsync();

            return ResultStatus.Success;
        }
    }
}
