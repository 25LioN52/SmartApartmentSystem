

namespace RaspberryIO
{
    public class RaspberryIo
    {
        //public int GetData(ModuleTypeEnum module, int data)
        //{
        //    lock (_lockObject)
        //    {
        //        var myDevice = Pi.I2C.AddDevice(0x20);

        //        // Simple Write and Read (there are algo register read and write methods)
        //        myDevice.Write(0x44);
        //        var response = myDevice.Read();
        //    }
        //}
        //void trr()
        //{
        //    var i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId: 1, deviceAddress: Mpr121.DefaultI2cAddress));
        //    var tt = new UnixI2cDevice();
        //    // Initialize controller with default configuration and auto-refresh the channel statuses every 100 ms.
        //    var mpr121 = new Mpr121(device: i2cDevice, periodRefresh: 100);

        //    // Subscribe to channel statuses updates.
        //    mpr121.ChannelStatusesChanged += (object sender, ChannelStatusesChangedEventArgs e) =>
        //    {
        //        var channelStatuses = e.ChannelStatuses;
        //        // do something.
        //    };
        //}
    }
}
