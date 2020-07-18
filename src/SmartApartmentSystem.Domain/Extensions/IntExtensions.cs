using SmartApartmentSystem.Domain.Light;
using SmartApartmentSystem.Domain.WaterTemperature;
using System;

namespace SmartApartmentSystem.Domain.Extensions
{
    public static class IntExtensions
    {
        private const byte MaxDeviceChannels = 100;
        public static byte ToGlobalType<T>(this T deviceType) where T : Enum =>
            deviceType switch
            {
                WaterTempChannels temp => (byte)temp,
                LightChannels light => (byte)(light + MaxDeviceChannels),
                _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(deviceType)),
            };

        public static T ToLocalType<T>(this int deviceType) where T : Enum => (T)Convert.ChangeType(deviceType % MaxDeviceChannels, typeof(T));
    }
}
