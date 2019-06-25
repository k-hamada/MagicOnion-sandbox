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

    GameObject cube;

    // StreamingHub クライアントで使用する gRPC チャネルを生成
    readonly Channel channel = new Channel("0.0.0.0", 12345, ChannelCredentials.Insecure);

    // StreamingHub サーバと通信を行うためのクライアント生成
    readonly GamingHubClient client = new GamingHubClient();

    void Awake()
    {
        m_UserName = DateTime.UtcNow.ToLongTimeString();
    }

    async Task Start()
    {
        // ゲーム起動時に設定した部屋名のルームに設定したユーザ名で入室する。
        cube = await client.ConnectAsync(channel, m_RoomName, m_UserName);
    }

    // Update is called once per frame
    void Update()
    {
        if (cube == null) return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        cube.gameObject.transform.Rotate(0, x, 0);
        cube.gameObject.transform.Translate(0, 0, z);

        // 毎フレームプレイヤーの位置(Vector3) と回転(Quaternion) を更新し、
        // ルームに入室している他ユーザ全員にブロードキャスト送信する
        client.MoveAsync(cube.gameObject.transform.position, cube.gameObject.transform.rotation);
    }

    async Task OnDestroy() {
        // GameClient が破棄される際の StreamingHub クライアント及び gRPC チャネルの解放処理
        await client.LeaveAsync();
        await client.DisposeAsync();
        await channel.ShutdownAsync();
    }
}
