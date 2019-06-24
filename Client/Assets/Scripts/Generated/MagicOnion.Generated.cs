#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace MagicOnion
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::MagicOnion;
    using global::MagicOnion.Client;

    public static partial class MagicOnionInitializer
    {
        static bool isRegistered = false;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            if(isRegistered) return;
            isRegistered = true;


            StreamingHubClientRegistry<Shared.Hubs.IGamingHub, Shared.Hubs.IGamingHubReceiver>.Register((a, _, b, c, d, e) => new Shared.Hubs.IGamingHubClient(a, b, c, d, e));
        }
    }
}

#pragma warning restore 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 612
#pragma warning restore 618
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace MagicOnion.Resolvers
{
    using System;
    using MessagePack;

    public class MagicOnionResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new MagicOnionResolver();

        MagicOnionResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = MagicOnionResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class MagicOnionResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static MagicOnionResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(3)
            {
                {typeof(global::MagicOnion.DynamicArgumentTuple<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>), 0 },
                {typeof(global::MagicOnion.DynamicArgumentTuple<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>), 1 },
                {typeof(global::Shared.MessagePackObjects.Player[]), 2 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new global::MagicOnion.DynamicArgumentTupleFormatter<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(default(global::UnityEngine.Vector3), default(global::UnityEngine.Quaternion));
                case 1: return new global::MagicOnion.DynamicArgumentTupleFormatter<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(default(string), default(string), default(global::UnityEngine.Vector3), default(global::UnityEngine.Quaternion));
                case 2: return new global::MessagePack.Formatters.ArrayFormatter<global::Shared.MessagePackObjects.Player>();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 612
#pragma warning restore 618
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Shared.Hubs {
    using Grpc.Core;
    using Grpc.Core.Logging;
    using MagicOnion;
    using MagicOnion.Client;
    using MessagePack;
    using System;
    using System.Threading.Tasks;

    public class IGamingHubClient : StreamingHubClientBase<global::Shared.Hubs.IGamingHub, global::Shared.Hubs.IGamingHubReceiver>, global::Shared.Hubs.IGamingHub
    {
        static readonly Method<byte[], byte[]> method = new Method<byte[], byte[]>(MethodType.DuplexStreaming, "IGamingHub", "Connect", MagicOnionMarshallers.ThroughMarshaller, MagicOnionMarshallers.ThroughMarshaller);

        protected override Method<byte[], byte[]> DuplexStreamingAsyncMethod { get { return method; } }

        readonly global::Shared.Hubs.IGamingHub __fireAndForgetClient;

        public IGamingHubClient(CallInvoker callInvoker, string host, CallOptions option, IFormatterResolver resolver, ILogger logger)
            : base(callInvoker, host, option, resolver, logger)
        {
            this.__fireAndForgetClient = new FireAndForgetClient(this);
        }
        
        public global::Shared.Hubs.IGamingHub FireAndForget()
        {
            return __fireAndForgetClient;
        }

        protected override void OnBroadcastEvent(int methodId, ArraySegment<byte> data)
        {
            switch (methodId)
            {
                case -1297457280: // OnJoin
                {
                    var result = LZ4MessagePackSerializer.Deserialize<global::Shared.MessagePackObjects.Player>(data, resolver);
                    receiver.OnJoin(result); break;
                }
                case 532410095: // OnLeave
                {
                    var result = LZ4MessagePackSerializer.Deserialize<global::Shared.MessagePackObjects.Player>(data, resolver);
                    receiver.OnLeave(result); break;
                }
                case 1429874301: // OnMove
                {
                    var result = LZ4MessagePackSerializer.Deserialize<global::Shared.MessagePackObjects.Player>(data, resolver);
                    receiver.OnMove(result); break;
                }
                default:
                    break;
            }
        }

        protected override void OnResponseEvent(int methodId, object taskCompletionSource, ArraySegment<byte> data)
        {
            switch (methodId)
            {
                case -733403293: // JoinAsync
                {
                    var result = LZ4MessagePackSerializer.Deserialize<global::Shared.MessagePackObjects.Player[]>(data, resolver);
                    ((TaskCompletionSource<global::Shared.MessagePackObjects.Player[]>)taskCompletionSource).TrySetResult(result);
                    break;
                }
                case 1368362116: // LeaveAsync
                {
                    var result = LZ4MessagePackSerializer.Deserialize<Nil>(data, resolver);
                    ((TaskCompletionSource<Nil>)taskCompletionSource).TrySetResult(result);
                    break;
                }
                case -99261176: // MoveAsync
                {
                    var result = LZ4MessagePackSerializer.Deserialize<Nil>(data, resolver);
                    ((TaskCompletionSource<Nil>)taskCompletionSource).TrySetResult(result);
                    break;
                }
                default:
                    break;
            }
        }
   
        public global::System.Threading.Tasks.Task<global::Shared.MessagePackObjects.Player[]> JoinAsync(string roomName, string userName, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation)
        {
            return WriteMessageWithResponseAsync<DynamicArgumentTuple<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>, global::Shared.MessagePackObjects.Player[]> (-733403293, new DynamicArgumentTuple<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(roomName, userName, position, rotation));
        }

        public global::System.Threading.Tasks.Task LeaveAsync()
        {
            return WriteMessageWithResponseAsync<Nil, Nil>(1368362116, Nil.Default);
        }

        public global::System.Threading.Tasks.Task MoveAsync(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation)
        {
            return WriteMessageWithResponseAsync<DynamicArgumentTuple<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>, Nil>(-99261176, new DynamicArgumentTuple<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(position, rotation));
        }


        class FireAndForgetClient : global::Shared.Hubs.IGamingHub
        {
            readonly IGamingHubClient __parent;

            public FireAndForgetClient(IGamingHubClient parentClient)
            {
                this.__parent = parentClient;
            }

            public global::Shared.Hubs.IGamingHub FireAndForget()
            {
                throw new NotSupportedException();
            }

            public Task DisposeAsync()
            {
                throw new NotSupportedException();
            }

            public Task WaitForDisconnect()
            {
                throw new NotSupportedException();
            }

            public global::System.Threading.Tasks.Task<global::Shared.MessagePackObjects.Player[]> JoinAsync(string roomName, string userName, global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation)
            {
                return __parent.WriteMessageAsyncFireAndForget<DynamicArgumentTuple<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>, global::Shared.MessagePackObjects.Player[]> (-733403293, new DynamicArgumentTuple<string, string, global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(roomName, userName, position, rotation));
            }

            public global::System.Threading.Tasks.Task LeaveAsync()
            {
                return __parent.WriteMessageAsync<Nil>(1368362116, Nil.Default);
            }

            public global::System.Threading.Tasks.Task MoveAsync(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation)
            {
                return __parent.WriteMessageAsync<DynamicArgumentTuple<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>>(-99261176, new DynamicArgumentTuple<global::UnityEngine.Vector3, global::UnityEngine.Quaternion>(position, rotation));
            }

        }
    }
}

#pragma warning restore 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
