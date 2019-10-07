using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"string\"][][\"byte[]\"][\"int\"][\"float\"][\"int\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"data\"][][\"bitmap\"][\"id\"][\"amount\"][\"num\"]]")]
	public abstract partial class NetworkManagerBehavior : NetworkBehavior
	{
		public const byte RPC_SEND_PATH = 0 + 5;
		public const byte RPC_RESET_LEVEL = 1 + 5;
		public const byte RPC_VICTORY = 2 + 5;
		public const byte RPC_SET_LEVEL = 3 + 5;
		public const byte RPC_SET_INK_LEVEL = 4 + 5;
		public const byte RPC_SET_ATTEMPTS = 5 + 5;
		
		public NetworkManagerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (NetworkManagerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("sendPath", sendPath, typeof(string));
			networkObject.RegisterRpc("resetLevel", resetLevel);
			networkObject.RegisterRpc("victory", victory, typeof(byte[]));
			networkObject.RegisterRpc("setLevel", setLevel, typeof(int));
			networkObject.RegisterRpc("setInkLevel", setInkLevel, typeof(float));
			networkObject.RegisterRpc("setAttempts", setAttempts, typeof(int));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new NetworkManagerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new NetworkManagerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// string data
		/// </summary>
		public abstract void sendPath(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void resetLevel(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void victory(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void setLevel(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void setInkLevel(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void setAttempts(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}