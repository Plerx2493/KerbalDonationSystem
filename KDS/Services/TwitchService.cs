using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace KDS.Services;

public class TwitchService : IHostedService
{
    private static TwitchPubSub client;
    
    public TwitchService()
    {
        
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        client = new TwitchPubSub();

        client.OnPubSubServiceConnected += onPubSubServiceConnected;
        client.OnChannelPointsRewardRedeemed += onChannelPointsRewardRedeemed;
        client.OnStreamUp += onStreamUp;
        client.OnStreamDown += onStreamDown;

        client.ListenToVideoPlayback("channelUsername");
        client.ListenToChannelPoints("channelTwitchID");
            
        await client.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.DisconnectAsync();
    }

    private static void onChannelPointsRewardRedeemed(object? sender, OnChannelPointsRewardRedeemedArgs e)
    {
        throw new NotImplementedException();
    }

    private static void onPubSubServiceConnected(object? sender, EventArgs e)
    {
        // SendTopics accepts an oauth optionally, which is necessary for some topics
        client.SendTopics();
    }
        
    private static void onListenResponse(object? sender, OnListenResponseArgs e)
    {
        if (!e.Successful)
            throw new Exception($"Failed to listen! Response: {e.Response}");
    }

    private static void onStreamUp(object? sender, OnStreamUpArgs e)
    {
        Console.WriteLine($"Stream just went up! Play delay: {e.PlayDelay}, server time: {e.ServerTime}");
    }

    private static void onStreamDown(object? sender, OnStreamDownArgs e)
    {
        Console.WriteLine($"Stream just went down! Server time: {e.ServerTime}");
    }
}