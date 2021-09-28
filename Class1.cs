using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;
using System.IO;
using System.Net;
using UnityEngine;

namespace RGBMonkey
{
	[BepInPlugin("org.Sheriff.gorillatag.RGBMonkey", "RGB Monkey", "1.0.0")]
	public class RGBMonkey : BaseUnityPlugin
	{
		public static ConfigEntry<bool> enabled;
		public static ConfigEntry<bool> randomColor;
		public static ConfigEntry<float> cycleSpeed;
		private Color color = new Color(0, 0, 0);
		private float hue = 0f;
		private float timer = 0f;
		private float updateRate = 1 / 1;
		private float updateTimer = 0;

		private void Awake()
		{

			Debug.Log("Starting RGB Monkey");
			ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "RGBMonkey.cfg"), true);

			enabled = config.Bind<bool>("Config", "Enabled", true, "Whether the plugin is enabled or not");
			randomColor = config.Bind<bool>("Config", "RandomColor", false, "Whether to cycle through colours of rainbow or choose random colors");

		}

		public void Update()
		{
			updateTimer += Time.deltaTime;

			bool Enabled = PhotonNetwork.CurrentRoom == null ? false : !PhotonNetwork.CurrentRoom.IsVisible;
			if (Enabled)

			{
				if (enabled.Value)
				{
					if (randomColor.Value)
					{
						if (Time.time > timer)
						{
							timer = Time.time + cycleSpeed.Value;
						}

					}
					else
					{
						if (hue >= 1)
						{
							hue = 0;
						}

						hue += cycleSpeed.Value;
					}

					GorillaTagger.Instance.UpdateColor(this.color.r, this.color.g, this.color.b);
					GorillaTagger.Instance.myVRRig.photonView.RPC("InitializeNoobMaterial", RpcTarget.All, new object[]
					{
						color.r,
						color.g,
						color.b
					});

				}
			}

		}

	}
}
