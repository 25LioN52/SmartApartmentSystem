using System.Threading;
using System.Threading.Tasks;
using Domain.Entity.Enums;
using MediatR;
using Unosquare.RaspberryIO;

namespace Commands
{
    public class SetBoilerTemperatureCommandHandler : IRequestHandler<SetBoilerTemperatureCommand, ResultStatus>
    {
        public async Task<ResultStatus> Handle(SetBoilerTemperatureCommand request, CancellationToken cancellationToken)
        {
            var myDevice = Pi.I2C.AddDevice((int)ModuleTypeEnum.TempModule);

            // Simple Write and Read (there are algo register read and write methods)
            await myDevice.WriteAsync(0x44);
            //var response = myDevice.Read();

            return ResultStatus.Success;
        }
    }
}
