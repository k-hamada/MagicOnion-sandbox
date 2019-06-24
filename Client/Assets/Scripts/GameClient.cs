using System;
using System.Threading.Tasks;
using Grpc.Core;
using UnityEngine;

public class GameClient : MonoBehaviour
{
    // プレイヤーの Transform (今回はメインカメラの Transform を指定)
    [SerializeField]
    Transform m_PlayerTransform;

    string m_UserName;
    const string m_RoomName = "MO";

    // StreamingHub クライアントで使用する gRPC チャネルを生成
    readonly Channel channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);

    // StreamingHub サーバと通信を行うためのクライアント生成
    readonly GamingHubClient client = new GamingHubClient();

    void Awake()
    {
        m_UserName = DateTime.UtcNow.ToLongTimeString();
    }

    async Task Start()
    {
        // ゲーム起動時に設定した部屋名のルームに設定したユーザ名で入室する。
        await client.ConnectAsync(channel, m_RoomName, m_UserName);
    }

    // Update is called once per frame
    void Update()
    {
        // 毎フレームプレイヤーの位置(Vector3) と回転(Quaternion) を更新し、
        // ルームに入室している他ユーザ全員にブロードキャスト送信する
        client.MoveAsync(m_PlayerTransform.position, m_PlayerTransform.rotation);
    }

    async Task OnDestroy() {
        // GameClient が破棄される際の StreamingHub クライアント及び gRPC チャネルの解放処理
        await client.LeaveAsync();
        await client.DisposeAsync();
        await channel.ShutdownAsync();
    }
}
