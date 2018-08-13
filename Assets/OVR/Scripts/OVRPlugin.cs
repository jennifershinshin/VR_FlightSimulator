/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.3 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculus.com/licenses/LICENSE-3.3

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Runtime.InteropServices;

// Internal C# wrapper for OVRPlugin.

internal static class OVRPlugin
{
	public static readonly System.Version wrapperVersion = OVRP_1_7_0.version;

	private static System.Version _version;
	public static System.Version version
	{
		get {
			if (_version == null)
			{
				try
				{
					string pluginVersion = OVRP_1_1_0.ovrp_GetVersion();

					if (pluginVersion != null)
					{
						// Truncate unsupported trailing version info for System.Version. Original string is returned if not present.
						pluginVersion = pluginVersion.Split('-')[0];
						_version = new System.Version(pluginVersion);
					}
					else
					{
						_version = new System.Version(0, 0, 0);
					}
				}
				catch
				{
					_version = new System.Version(0, 0, 0);
				}

				// Unity 5.1.1f3-p3 have OVRPlugin version "0.5.0", which isn't accurate.
				if (_version == OVRP_0_5_0.version)
					_version = OVRP_0_1_0.version;

				if (_version < OVRP_1_3_0.version)
					throw new PlatformNotSupportedException("Oculus Utilities version " + wrapperVersion + " is too new for OVRPlugin version " + _version.ToString () + ". Update to the latest version of Unity.");
			}

			return _version;
		}
	}

	private static System.Version _nativeSDKVersion;
	public static System.Version nativeSDKVersion
	{
		get {
			if (_nativeSDKVersion == null)
			{
				try
				{
					string sdkVersion = string.Empty;

					if (version >= OVRP_1_1_0.version)
						sdkVersion = OVRP_1_1_0.ovrp_GetNativeSDKVersion();
					else
						sdkVersion = "0.0.0";
                                    
					if (sdkVersion != null)
					{
						// Truncate unsupported trailing version info for System.Version. Original string is returned if not present.
						sdkVersion = sdkVersion.Split('-')[0];
						_nativeSDKVersion = new System.Version(sdkVersion);
					}
					else
					{
						_nativeSDKVersion = new System.Version(0, 0, 0);
					}
				}
				catch
				{
					_nativeSDKVersion = new System.Version(0, 0, 0);
				}
			}

			return _nativeSDKVersion;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct GUID
	{
		public int a;
		public short b;
		public short c;
		public byte d0;
		public byte d1;
		public byte d2;
		public byte d3;
		public byte d4;
		public byte d5;
		public byte d6;
		public byte d7;
	}

	public enum Bool
	{
		False = 0,
		True
	}

	public enum Eye
	{
		None  = -1,
		Left  = 0,
		Right = 1,
		Count = 2
	}

	public enum Tracker
	{
		None   = -1,
		Zero   = 0,
		One    = 1,
		Count,
	}

	public enum Node
	{
		None           = -1,
		EyeLeft        = 0,
		EyeRight       = 1,
		EyeCenter      = 2,
		HandLeft       = 3,
		HandRight      = 4,
		TrackerZero    = 5,
		TrackerOne     = 6,
		Count,
	}

	public enum Controller
	{
		None           = 0,
		LTouch         = 0x00000001,
		RTouch         = 0x00000002,
		Touch          = LTouch | RTouch,
		Remote         = 0x00000004,
		Gamepad        = 0x00000008,
		Active         = unchecked((int)0x80000000),
		All            = ~None,
	}

	public enum TrackingOrigin
	{
		EyeLevel       = 0,
		FloorLevel     = 1,
		Count,
	}

	public enum RecenterFlags
	{
		Default        = 0,
		IgnoreAll      = unchecked((int)0x80000000),
		Count,
	}

	public enum BatteryStatus
	{
		Charging = 0,
		Discharging,
		Full,
		NotCharging,
		Unknown,
	}

	public enum PlatformUI
	{
		None = -1,
		GlobalMenu = 0,
		ConfirmQuit,
        GlobalMenuTutorial,
	}

	public enum SystemRegion
	{
		Unspecified = 0,
		Japan,
	}

	private const int OverlayShapeFlagShift = 4;
	private enum OverlayFlag
	{
		None        = unchecked((int)0x00000000),
		OnTop       = unchecked((int)0x00000001),
		HeadLocked  = unchecked((int)0x00000002),

		// Using the 5-8 bits for shapes, total 16 potential shapes can be supported 0x000000[0]0 ->  0x000000[F]0
		ShapeFlag_Quad      = unchecked((int)OverlayShape.Quad << OverlayShapeFlagShift),
		ShapeFlag_Cylinder  = unchecked((int)OverlayShape.Cylinder << OverlayShapeFlagShift),
		ShapeFlag_Cubemap   = unchecked((int)OverlayShape.Cubemap << OverlayShapeFlagShift),
		ShapeFlagRangeMask  = unchecked((int)0xF << OverlayShapeFlagShift),
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2i
	{
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2f
	{
		public float x;
		public float y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3f
	{
		public float x;
		public float y;
		public float z;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Quatf
	{
		public float x;
		public float y;
		public float z;
		public float w;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Posef
	{
		public Quatf Orientation;
		public Vector3f Position;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct InputState
	{
		public uint ConnectedControllers;
		public uint Buttons;
		public uint Touches;
		public uint NearTouches;
		public float LIndexTrigger;
		public float RIndexTrigger;
		public float LHandTrigger;
		public float RHandTrigger;
		public Vector2f LThumbstick;
		public Vector2f RThumbstick;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ControllerState
	{
		public uint ConnectedControllers;
		public uint Buttons;
		public uint Touches;
		public uint NearTouches;
		public float LIndexTrigger;
		public float RIndexTrigger;
		public float LHandTrigger;
		public float RHandTrigger;
		public Vector2f LThumbstick;
		public Vector2f RThumbstick;

		// maintain backwards compat for OVRP_0_1_2.ovrp_GetInputState()
		internal ControllerState(InputState inputState)
		{
			ConnectedControllers = inputState.ConnectedControllers;
			Buttons              = inputState.Buttons;
			Touches              = inputState.Touches;
			NearTouches          = inputState.NearTouches;
			LIndexTrigger        = inputState.LIndexTrigger;
			RIndexTrigger        = inputState.RIndexTrigger;
			LHandTrigger         = inputState.LHandTrigger;
			RHandTrigger         = inputState.RHandTrigger;
			LThumbstick          = inputState.LThumbstick;
			RThumbstick          = inputState.RThumbstick;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HapticsBuffer
	{
		public IntPtr Samples;
		public int SamplesCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HapticsState
	{
		public int SamplesAvailable;
		public int SamplesQueued;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HapticsDesc
	{
		public int SampleRateHz;
		public int SampleSizeInBytes;
		public int MinimumSafeSamplesQueued;
		public int MinimumBufferSamplesCount;
		public int OptimalBufferSamplesCount;
		public int MaximumBufferSamplesCount;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Sizei
	{
		public int w;
		public int h;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Frustumf
	{
		public float zNear;
		public float zFar;
		public float fovX;
		public float fovY;
	}

	public static bool initialized
	{
		get {
				return OVRP_1_1_0.ovrp_GetInitialized() == OVRPlugin.Bool.True;
		}
	}

	public static bool chromatic
	{
		get {
			if (version >= OVRP_1_7_0.version)
				return OVRP_1_7_0.ovrp_GetAppChromaticCorrection() == OVRPlugin.Bool.True;
		
#if UNITY_ANDROID && !UNITY_EDITOR
			return false;
#else
			return true;
#endif
		}

		set {
			if (version >= OVRP_1_7_0.version)
				OVRP_1_7_0.ovrp_SetAppChromaticCorrection(ToBool(value));
		}
	}

	public static bool monoscopic
	{
		get { return OVRP_1_1_0.ovrp_GetAppMonoscopic() == OVRPlugin.Bool.True; }
		set { OVRP_1_1_0.ovrp_SetAppMonoscopic(ToBool(value)); }
	}

	public static bool rotation
	{
		get { return OVRP_1_1_0.ovrp_GetTrackingOrientationEnabled() == Bool.True; }
		set { OVRP_1_1_0.ovrp_SetTrackingOrientationEnabled(ToBool(value)); }
	}

	public static bool position
	{
		get { return OVRP_1_1_0.ovrp_GetTrackingPositionEnabled() == Bool.True; }
		set { OVRP_1_1_0.ovrp_SetTrackingPositionEnabled(ToBool(value)); }
	}

	public static bool useIPDInPositionTracking
	{
		get {
			if (version >= OVRP_1_6_0.version)
				return OVRP_1_6_0.ovrp_GetTrackingIPDEnabled() == OVRPlugin.Bool.True;

			return true;
		}

		set {
			if (version >= OVRP_1_6_0.version)
				OVRP_1_6_0.ovrp_SetTrackingIPDEnabled(ToBool(value));
		}
	}

	public static bool positionSupported { get { return OVRP_1_1_0.ovrp_GetTrackingPositionSupported() == Bool.True; } }

	public static bool positionTracked { get { return OVRP_1_1_0.ovrp_GetNodePositionTracked(Node.EyeCenter) == Bool.True; } }

	public static bool powerSaving { get { return OVRP_1_1_0.ovrp_GetSystemPowerSavingMode() == Bool.True; } }

	public static bool hmdPresent { get { return OVRP_1_1_0.ovrp_GetNodePresent(Node.EyeCenter) == Bool.True; } }

	public static bool userPresent { get { return OVRP_1_1_0.ovrp_GetUserPresent() == Bool.True; } }

	public static bool headphonesPresent { get { return OVRP_1_3_0.ovrp_GetSystemHeadphonesPresent() == OVRPlugin.Bool.True; } }

	public static int recommendedMSAALevel
	{
		get {
			if (version >= OVRP_1_6_0.version)
				return OVRP_1_6_0.ovrp_GetSystemRecommendedMSAALevel ();
			else
				return 2;
		}
	}

	public static SystemRegion systemRegion
	{
		get {
			if (version >= OVRP_1_5_0.version)
				return OVRP_1_5_0.ovrp_GetSystemRegion();
			else
				return SystemRegion.Unspecified;
		}
	}

	private static Guid _cachedAudioOutGuid;
	private static string _cachedAudioOutString;
	public static string audioOutId
	{
		get
		{
				try
				{
					IntPtr ptr = OVRP_1_1_0.ovrp_GetAudioOutId();
					if (ptr != IntPtr.Zero)
					{
						GUID nativeGuid = (GUID)Marshal.PtrToStructure(ptr, typeof(OVRPlugin.GUID));
						Guid managedGuid = new Guid(
								nativeGuid.a,
								nativeGuid.b,
								nativeGuid.c,
								nativeGuid.d0,
								nativeGuid.d1,
								nativeGuid.d2,
								nativeGuid.d3,
								nativeGuid.d4,
								nativeGuid.d5,
								nativeGuid.d6,
								nativeGuid.d7);

						if (managedGuid != _cachedAudioOutGuid)
						{
							_cachedAudioOutGuid = managedGuid;
							_cachedAudioOutString = _cachedAudioOutGuid.ToString();
						}

						return _cachedAudioOutString;
					}
				}
			catch {}

					return string.Empty;
				}
			}

	private static Guid _cachedAudioInGuid;
	private static string _cachedAudioInString;
	public static string audioInId
	{
		get
		{
				try
				{
					IntPtr ptr = OVRP_1_1_0.ovrp_GetAudioInId();
					if (ptr != IntPtr.Zero)
					{
						GUID nativeGuid = (GUID)Marshal.PtrToStructure(ptr, typeof(OVRPlugin.GUID));
						Guid managedGuid = new Guid(
								nativeGuid.a,
								nativeGuid.b,
								nativeGuid.c,
								nativeGuid.d0,
								nativeGuid.d1,
								nativeGuid.d2,
								nativeGuid.d3,
								nativeGuid.d4,
								nativeGuid.d5,
								nativeGuid.d6,
								nativeGuid.d7);

						if (managedGuid != _cachedAudioInGuid)
						{
							_cachedAudioInGuid = managedGuid;
							_cachedAudioInString = _cachedAudioInGuid.ToString();
						}

						return _cachedAudioInString;
					}
				}
			catch {}

			return string.Empty;
		}
	}

	public static bool hasVrFocus { get { return OVRP_1_1_0.ovrp_GetAppHasVrFocus() == Bool.True; } }

	public static bool shouldQuit { get { return OVRP_1_1_0.ovrp_GetAppShouldQuit() == Bool.True; } }

	public static bool shouldRecenter { get { return OVRP_1_1_0.ovrp_GetAppShouldRecenter() == Bool.True; } }

	public static string productName { get { return OVRP_1_1_0.ovrp_GetSystemProductName(); } }

	public static string latency { get { return OVRP_1_1_0.ovrp_GetAppLatencyTimings(); } }

	public static float eyeDepth
	{
		get { return OVRP_1_1_0.ovrp_GetUserEyeDepth(); }
		set { OVRP_1_1_0.ovrp_SetUserEyeDepth(value); }
	}

	public static float eyeHeight
	{
		get { return OVRP_1_1_0.ovrp_GetUserEyeHeight(); }
		set { OVRP_1_1_0.ovrp_SetUserEyeHeight(value); }
	}

	public static float batteryLevel
	{
		get { return OVRP_1_1_0.ovrp_GetSystemBatteryLevel(); }
	}

	public static float batteryTemperature
	{
		get { return OVRP_1_1_0.ovrp_GetSystemBatteryTemperature(); }
	}

	public static int cpuLevel
	{
		get { return OVRP_1_1_0.ovrp_GetSystemCpuLevel(); }
		set { OVRP_1_1_0.ovrp_SetSystemCpuLevel(value); }
	}

	public static int gpuLevel
	{
		get { return OVRP_1_1_0.ovrp_GetSystemGpuLevel(); }
		set { OVRP_1_1_0.ovrp_SetSystemGpuLevel(value); }
	}

	public static int vsyncCount
	{
		get { return OVRP_1_1_0.ovrp_GetSystemVSyncCount(); }
		set { OVRP_1_2_0.ovrp_SetSystemVSyncCount(value); }
		}

	public static float systemVolume
	{
		get { return OVRP_1_1_0.ovrp_GetSystemVolume(); }
	}

	public static float ipd
	{
		get { return OVRP_1_1_0.ovrp_GetUserIPD(); }
		set { OVRP_1_1_0.ovrp_SetUserIPD(value); }
	}

	public static bool occlusionMesh
	{
		get { return OVRP_1_3_0.ovrp_GetEyeOcclusionMeshEnabled() == Bool.True; }
		set { OVRP_1_3_0.ovrp_SetEyeOcclusionMeshEnabled(ToBool(value)); }
	}

	public static BatteryStatus batteryStatus
	{
		get { return OVRP_1_1_0.ovrp_GetSystemBatteryStatus(); }
	}

	public static Posef GetEyeVelocity(Eye eyeId) { return GetNodeVelocity((Node)eyeId); }
	public static Posef GetEyeAcceleration(Eye eyeId) { return GetNodeAcceleration((Node)eyeId); }
	public static Frustumf GetEyeFrustum(Eye eyeId) { return OVRP_1_1_0.ovrp_GetNodeFrustum((Node)eyeId); }
	public static Sizei GetEyeTextureSize(Eye eyeId) { return OVRP_0_1_0.ovrp_GetEyeTextureSize(eyeId); }
	public static Posef GetTrackerPose(Tracker trackerId) { return GetNodePose((Node)((int)trackerId + (int)Node.TrackerZero)); }
	public static Frustumf GetTrackerFrustum(Tracker trackerId) { return OVRP_1_1_0.ovrp_GetNodeFrustum((Node)((int)trackerId + (int)Node.TrackerZero)); }
	public static bool ShowUI(PlatformUI ui) { return OVRP_1_1_0.ovrp_ShowSystemUI(ui) == Bool.True; }
	public static bool SetOverlayQuad(bool onTop, bool headLocked, IntPtr texture, IntPtr device, Posef pose, Vector3f scale, int layerIndex=0, OverlayShape shape=OverlayShape.Quad)
	{
		if (version >= OVRP_1_6_0.version)
		{
			uint flags = (uint)OverlayFlag.None;
			if (onTop)
				flags |= (uint)OverlayFlag.OnTop;
			if (headLocked)
				flags |= (uint)OverlayFlag.HeadLocked;

			if (shape == OverlayShape.Cylinder || shape == OverlayShape.Cubemap)
			{
#if UNITY_ANDROID
				if (version >= OVRP_1_7_0.version)
					flags |= (uint)(shape) << OverlayShapeFlagShift;
				else
#endif
					return false;
			}
			return OVRP_1_6_0.ovrp_SetOverlayQuad3(flags, texture, IntPtr.Zero, device, pose, scale, layerIndex) == Bool.True;
		}

		if (layerIndex != 0)
			return false;
		
			return OVRP_0_1_1.ovrp_SetOverlayQuad2(ToBool(onTop), ToBool(headLocked), texture, device, pose, scale) == Bool.True;
	}

	public static Posef GetNodePose(Node nodeId)
	{
		return OVRP_0_1_2.ovrp_GetNodePose(nodeId);
	}

	public static Posef GetNodeVelocity(Node nodeId)
	{
		return OVRP_0_1_3.ovrp_GetNodeVelocity(nodeId);
	}

	public static Posef GetNodeAcceleration(Node nodeId)
	{
		return OVRP_0_1_3.ovrp_GetNodeAcceleration(nodeId);
	}

	public static bool GetNodePresent(Node nodeId)
	{
		return OVRP_1_1_0.ovrp_GetNodePresent(nodeId) == Bool.True;
	}

	public static bool GetNodeOrientationTracked(Node nodeId)
	{
		return OVRP_1_1_0.ovrp_GetNodeOrientationTracked(nodeId) == Bool.True;
	}

	public static bool GetNodePositionTracked(Node nodeId)
	{
		return OVRP_1_1_0.ovrp_GetNodePositionTracked(nodeId) == Bool.True;
	}

	public static ControllerState GetControllerState(uint controllerMask)
	{
		return OVRP_1_1_0.ovrp_GetControllerState(controllerMask);
	}

	public static bool SetControllerVibration(uint controllerMask, float frequency, float amplitude)
	{
		return OVRP_0_1_2.ovrp_SetControllerVibration(controllerMask, frequency, amplitude) == Bool.True;
	}

	public static HapticsDesc GetControllerHapticsDesc(uint controllerMask)
	{
		if (version >= OVRP_1_6_0.version)
		{
			return OVRP_1_6_0.ovrp_GetControllerHapticsDesc(controllerMask);
		}
		else
		{
			return new HapticsDesc();
		}
	}

	public static HapticsState GetControllerHapticsState(uint controllerMask)
	{
		if (version >= OVRP_1_6_0.version)
		{
			return OVRP_1_6_0.ovrp_GetControllerHapticsState(controllerMask);
		}
		else
		{
			return new HapticsState();
		}
	}

	public static bool SetControllerHaptics(uint controllerMask, HapticsBuffer hapticsBuffer)
	{
		if (version >= OVRP_1_6_0.version)
		{
			return OVRP_1_6_0.ovrp_SetControllerHaptics(controllerMask, hapticsBuffer) == Bool.True;
		}
		else
		{
			return false;
		}
	}

	public static float GetEyeRecommendedResolutionScale()
	{
		if (version >= OVRP_1_6_0.version)
		{
			return OVRP_1_6_0.ovrp_GetEyeRecommendedResolutionScale();
		}
		else
		{
			return 1.0f;
		}
	}

	public static float GetAppCpuStartToGpuEndTime()
	{
		if (version >= OVRP_1_6_0.version)
		{
			return OVRP_1_6_0.ovrp_GetAppCpuStartToGpuEndTime();
		}
		else
		{
			return 0.0f;
		}
	}

	private static Bool ToBool(bool b)
	{
		return (b) ? OVRPlugin.Bool.True : OVRPlugin.Bool.False;
	}

	public static TrackingOrigin GetTrackingOriginType()
	{
		return OVRP_1_0_0.ovrp_GetTrackingOriginType();
	}

	public static bool SetTrackingOriginType(TrackingOrigin originType)
	{
		return OVRP_1_0_0.ovrp_SetTrackingOriginType(originType) == Bool.True;
	}

	public static Posef GetTrackingCalibratedOrigin()
	{
		return OVRP_1_0_0.ovrp_GetTrackingCalibratedOrigin();
	}

	public static bool SetTrackingCalibratedOrigin()
	{
		return OVRP_1_2_0.ovrpi_SetTrackingCalibratedOrigin() == Bool.True;
	}

	public static bool RecenterTrackingOrigin(RecenterFlags flags)
	{
		return OVRP_1_0_0.ovrp_RecenterTrackingOrigin((uint)flags) == Bool.True;
	}
	
	//HACK: This makes Unity think it always has VR focus while OVRPlugin.cs reports the correct value.
	internal static bool ignoreVrFocus
	{
		set { OVRP_1_2_1.ovrp_SetAppIgnoreVrFocus(ToBool(value)); }
	}

	private const string pluginName = "OVRPlugin";

	private static class OVRP_0_1_0
	{
		public static readonly System.Version version = new System.Version(0, 1, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Sizei ovrp_GetEyeTextureSize(Eye eyeId);
	}

	private static class OVRP_0_1_1
	{
		public static readonly System.Version version = new System.Version(0, 1, 1);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetOverlayQuad2(Bool onTop, Bool headLocked, IntPtr texture, IntPtr device, Posef pose, Vector3f scale);
	}

	private static class OVRP_0_1_2
	{
		public static readonly System.Version version = new System.Version(0, 1, 2);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Posef ovrp_GetNodePose(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetControllerVibration(uint controllerMask, float frequency, float amplitude);
	}

	private static class OVRP_0_1_3
	{
		public static readonly System.Version version = new System.Version(0, 1, 3);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Posef ovrp_GetNodeVelocity(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Posef ovrp_GetNodeAcceleration(Node nodeId);
	}

	private static class OVRP_0_5_0
	{
		public static readonly System.Version version = new System.Version(0, 5, 0);
	}

	private static class OVRP_1_0_0
	{
		public static readonly System.Version version = new System.Version(1, 0, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern TrackingOrigin ovrp_GetTrackingOriginType();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetTrackingOriginType(TrackingOrigin originType);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Posef ovrp_GetTrackingCalibratedOrigin();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_RecenterTrackingOrigin(uint flags);
	}

	private static class OVRP_1_1_0
	{
		public static readonly System.Version version = new System.Version(1, 1, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetInitialized();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetVersion")]
		private static extern IntPtr _ovrp_GetVersion();
		public static string ovrp_GetVersion() { return Marshal.PtrToStringAnsi(_ovrp_GetVersion()); }

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetNativeSDKVersion")]
		private static extern IntPtr _ovrp_GetNativeSDKVersion();
		public static string ovrp_GetNativeSDKVersion() { return Marshal.PtrToStringAnsi(_ovrp_GetNativeSDKVersion()); }

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioOutId();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioInId();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeTextureScale();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetEyeTextureScale(float value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetTrackingOrientationSupported();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetTrackingOrientationEnabled();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetTrackingOrientationEnabled(Bool value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetTrackingPositionSupported();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetTrackingPositionEnabled();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetTrackingPositionEnabled(Bool value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetNodePresent(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetNodeOrientationTracked(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetNodePositionTracked(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Frustumf ovrp_GetNodeFrustum(Node nodeId);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ControllerState ovrp_GetControllerState(uint controllerMask);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemCpuLevel();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetSystemCpuLevel(int value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemGpuLevel();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetSystemGpuLevel(int value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetSystemPowerSavingMode();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemDisplayFrequency();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemVSyncCount();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemVolume();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern BatteryStatus ovrp_GetSystemBatteryStatus();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryLevel();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryTemperature();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetSystemProductName")]
		private static extern IntPtr _ovrp_GetSystemProductName();
		public static string ovrp_GetSystemProductName() { return Marshal.PtrToStringAnsi(_ovrp_GetSystemProductName()); }

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_ShowSystemUI(PlatformUI ui);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetAppMonoscopic();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetAppMonoscopic(Bool value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetAppHasVrFocus();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetAppShouldQuit();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetAppShouldRecenter();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetAppLatencyTimings")]
		private static extern IntPtr _ovrp_GetAppLatencyTimings();
		public static string ovrp_GetAppLatencyTimings() { return Marshal.PtrToStringAnsi(_ovrp_GetAppLatencyTimings()); }

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetUserPresent();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserIPD();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetUserIPD(float value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeDepth();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetUserEyeDepth(float value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeHeight();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetUserEyeHeight(float value);
	}

	private static class OVRP_1_2_0
	{
		public static readonly System.Version version = new System.Version(1, 2, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetSystemVSyncCount(int vsyncCount);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrpi_SetTrackingCalibratedOrigin();
	}

	private static class OVRP_1_2_1
	{
		public static readonly System.Version version = new System.Version(1, 2, 1);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetAppIgnoreVrFocus(Bool value);
	}

	private static class OVRP_1_3_0
	{
		public static readonly System.Version version = new System.Version(1, 3, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetEyeOcclusionMeshEnabled();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetEyeOcclusionMeshEnabled(Bool value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetSystemHeadphonesPresent();
	}

	private static class OVRP_1_5_0
	{
		public static readonly System.Version version = new System.Version(1, 5, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SystemRegion ovrp_GetSystemRegion();
	}

	private static class OVRP_1_6_0
	{
		public static readonly System.Version version = new System.Version(1, 6, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetTrackingIPDEnabled();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetTrackingIPDEnabled(Bool value);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern HapticsDesc ovrp_GetControllerHapticsDesc(uint controllerMask);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern HapticsState ovrp_GetControllerHapticsState(uint controllerMask);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetControllerHaptics(uint controllerMask, HapticsBuffer hapticsBuffer);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetOverlayQuad3(uint flags, IntPtr textureLeft, IntPtr textureRight, IntPtr device, Posef pose, Vector3f scale, int layerIndex);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeRecommendedResolutionScale();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetAppCpuStartToGpuEndTime();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemRecommendedMSAALevel();
	}

	private static class OVRP_1_7_0
	{
		public static readonly System.Version version = new System.Version(1, 7, 0);

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_GetAppChromaticCorrection();

		[DllImport(pluginName, CallingConvention = CallingConvention.Cdecl)]
		public static extern Bool ovrp_SetAppChromaticCorrection(Bool value);
	}
}
