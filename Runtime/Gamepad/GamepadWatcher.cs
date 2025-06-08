using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad
{
	/// <summary>
	/// Selector/Watcher : manage list of targets taht will receive inputs
	/// transfert input to specific selected entities
	/// </summary>
	public class GamepadWatcher : MonoBehaviour
	{
		public bool verbose = false;

		[Header("refs")]

		public InputSysGamepad playerInputSys; // the bridge that will sub to InputSystem (specific to each player)

		/// <summary>
		/// aka listener
		/// </summary>
		WatcherInput<ISelectable> targets; // normal object
		WatcherInput<ISelectableAbsorb> absorbs; // object that can absorb

		/// <summary>
		/// anything sub to this list will lock inputs for this watcher
		/// </summary>
		List<object> lockers = new();
		public bool IsLocked => lockers.Count > 0;

		public bool CanReact
		{
			get
			{
				if (IsLocked) return false;
				return true;
			}
		}

		System.Guid _guid;
		public int Guid
		{
			get
			{
				if (_guid == null) _guid = System.Guid.NewGuid();
				return _guid.GetHashCode();
			}
		}

		public int PluggedIndex
		{
			get
			{
				return GamepadsWatcher.instance.getControllerIndex(this);
			}
		}

		//public bool isPlugged => pluggedIndex > -1;
		public bool IsPlugged
		{
			get
			{
				if (playerInputSys.sysDevice == null)
					return false;

				// keyboard doesn't support enabled state
				return true;
				//return playerInputSys.sysDevice.enabled;
			}
		}

		public bool isPrimary => playerInputSys.controllerType == InputSysGamepad.InputController.gamepad_0;

		private void Awake()
		{
			create();
		}

		virtual protected void create() { }

		private void Start()
		{
			targets = new WatcherInput<ISelectable>();
			absorbs = new WatcherInput<ISelectableAbsorb>();

			if (playerInputSys != null)
			{
				setupCallbacks();
			}

			setup();
		}

		virtual protected void setup()
		{ }

		void setupCallbacks()
		{
			var subs = playerInputSys.subs;

			subs.onJoystickPerformed += onJoystick;
			subs.onJoystickReleased += onJoystickRelease;
			subs.onJoystickDirection += onJoyDirection;
			subs.onJoystickPunchDirection += onJoyPunch;

			subs.onTriggerPerformed += onTrigger;
			subs.onButtonPerformed += onButton;

			//Debug.Log("watcher:ON");
		}

		public void clearCallbacks()
		{
			var subs = playerInputSys.subs;

			subs.onJoystickDirection -= onJoyDirection;
			subs.onJoystickPerformed -= onJoystick;

			subs.onJoystickReleased -= onJoystickRelease;
			subs.onTriggerPerformed -= onTrigger;
			subs.onButtonPerformed -= onButton;
		}

		public void setLock(object locker)
		{
			if (!lockers.Contains(locker)) lockers.Add(locker);
		}
		public void remLock(object locker)
		{
			if (lockers.Contains(locker)) lockers.Remove(locker);
		}

		public bool hasSelection() => targets.hasSomething();

		/// <summary>
		/// force all selected element to reset inputs
		/// 
		/// force tiny to reset joystick state (ie : when input block starts)
		/// </summary>
		public void bubbleNeutral()
		{
			// force a neutral (for motion)
			onJoystickRelease(InputJoystickSide.LEFT);

			// force a release
			onButton(InputActions.ACT_SOUTH, false);
		}

		public void queueReplace(ISelectable target)
		{
			targets.deselectAll();
			queueSelection(target);
		}

		/// <summary>
		/// par default le queue selection va cancel les inputs du previous
		/// </summary>
		public void queueSelection(ISelectable target, bool forceInputRelease = true)
		{
			if (target == null)
			{
				Debug.LogError("don't use queue for clearing, use deselect");
				return;
			}

			if (forceInputRelease)
			{
				bubbleNeutral();
			}

			if (target is ISelectableAbsorb)
			{
				if (absorbs.queueSelection(target as ISelectableAbsorb))
				{
					reactQueue(target);
				}
			}
			else
			{
				if (targets.queueSelection(target))
				{
					reactQueue(target);
				}
			}

		}

		virtual protected void reactQueue(ISelectable target)
		{ }

		public void unqueueSelection(ISelectable target)
		{
			if (target == null)
			{
				Debug.LogWarning("nothing given to unqeue ??");
				return;
			}

			if (target is ISelectableAbsorb)
			{
				if (absorbs.unqueueSelection(target as ISelectableAbsorb))
				{
					reactUnqueue(target);
				}
			}
			else
			{
				if (targets.unqueueSelection(target))
				{
					reactUnqueue(target);
				}
			}
		}

		/// <summary>
		/// from targets[]
		/// </summary>
		virtual protected void reactUnqueue(ISelectable target)
		{ }

		void onJoystickRelease(InputJoystickSide side)
		{
			if (!CanReact) return;

			// feed selection(s)
			onJoystick(side, Vector2.zero);

			// no need, managed by sys gamepad
			//onJoyDirection(side, Vector2.zero); // joy release
		}

		void onJoystick(InputJoystickSide side, Vector2 value)
		{
			if (!CanReact) return;

			log("joy." + side + "." + value);
			if (absorbs.onJoystick(side, value))
			{
				return;
			}

			targets.onJoystick(side, value);
		}

		void onJoyDirection(InputJoystickSide side, Vector2 value)
		{
			if (!CanReact) return;

			log("direction." + side + "." + value);

			if (absorbs.onJoystickDirection(side, value))
			{
				return;
			}

			targets.onJoystickDirection(side, value);
		}

		void onJoyPunch(InputJoystickSide side, Vector2 value)
		{
			if (!CanReact) return;

			log("punch." + side + "." + value);

			if (absorbs.onJoystickPunch(side, value)) return;
			targets.onJoystickPunch(side, value);
		}

		void onTrigger(InputTriggers side, float value)
		{
			if (!CanReact) return;

			log(side + "." + value);

			if (absorbs.onTrigger(side, value))
			{
				return;
			}

			targets.onTrigger(side, value);
		}

		private void onButton(InputActions type, bool status)
		{
			if (!CanReact) return;

			log("button." + type + "." + status);

			if (absorbs.onButton(type, status))
			{
				return;
			}

			targets.onButton(type, status);
		}

		public string stringify()
		{
			string ret = name;
			if (IsLocked) ret += " LOCKED";
			if (targets != null) ret += "\n" + targets.stringify();
			if (absorbs != null) ret += "\n" + absorbs.stringify();
			return ret;
		}

		void log(string msg)
		{
			if (!verbose) return;
			Debug.Log("[INPUT.WATCHER]      " + msg, this);
		}

		[ContextMenu("log")]
		public void cmLog()
		{
			Debug.Log(name, this);
			if (IsLocked) Debug.Log("LOCKED, x" + lockers.Count);
			if (targets != null) Debug.Log(targets.stringify());
			if (absorbs != null) Debug.Log(absorbs.stringify());
		}

		[ContextMenu("check plugged ?")]
		void cmLogPlugged()
		{
			Debug.Log(name);
			Debug.Log("device  : " + playerInputSys.sysDevice, this);
			Debug.Log("plugged : " + IsPlugged, this);
		}

	}
}
