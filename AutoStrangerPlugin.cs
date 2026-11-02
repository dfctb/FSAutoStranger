using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace FSAutoStranger
{
    [BepInPlugin("com.conf.fsautostranger", "FS AutoStranger", "0.1.0")]
    public class AutoStrangerPlugin : BaseUnityPlugin
    {
        private ConfigEntry<KeyboardShortcut> _toggleKey;
        private ConfigEntry<KeyboardShortcut> _summonKey;
        private ConfigEntry<bool> _enabledOnStart;
        private ConfigEntry<float> _interval;
        private ConfigEntry<float> _cooldown;

        private bool _enabled;
        private float _timer;
        private float _cooldownUntil;
        private bool _warnedMissingRoom;

        private void Awake()
        {
            _toggleKey = Config.Bind("Hotkeys", "ToggleKey", new KeyboardShortcut(KeyCode.F6),
                "Key that turns auto-catching on and off in game");
            _summonKey = Config.Bind("Hotkeys", "SummonStrangerKey", new KeyboardShortcut(KeyCode.None),
                "Optional testing key: makes the Mysterious Stranger appear right now (unset by default)");

            _enabledOnStart = Config.Bind("General", "EnabledOnStart", true,
                "Is auto-catching on when the game starts");
            _interval = Config.Bind("General", "CheckInterval", 0.5f,
                "Seconds between checks");
            _cooldown = Config.Bind("General", "MinSecondsBetweenCatches", 30f,
                new ConfigDescription("Safety pause (real seconds) after each catch",
                    new AcceptableValueRange<float>(5f, 600f)));

            _enabled = _enabledOnStart.Value;
            Logger.LogInfo("FS AutoStranger loaded (" + (_enabled ? "ON" : "OFF") + ")");
        }

        // Reads a property or field by name, including non-public ones and ones declared in base classes.
        private static object ReadMember(object target, string name)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            for (Type t = target.GetType(); t != null; t = t.BaseType)
            {
                PropertyInfo p = t.GetProperty(name, flags);
                if (p != null && p.GetIndexParameters().Length == 0)
                    return p.GetValue(target, null);

                FieldInfo f = t.GetField(name, flags);
                if (f != null)
                    return f.GetValue(target);
            }
            return null;
        }

        private void Update()
        {
            if (_toggleKey.Value.IsDown())
            {
                _enabled = !_enabled;
                Logger.LogInfo("AutoStranger: " + (_enabled ? "ON" : "OFF"));
            }

            if (_summonKey.Value.IsDown())
            {
                Summon();
            }

            if (!_enabled) return;

            _timer += Time.unscaledDeltaTime;
            if (_timer < _interval.Value) return;
            _timer = 0f;

            try
            {
                Catch();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        private void Summon()
        {
            try
            {
                if (!MonoSingleton<MysteriousStrangerMgr>.IsInstanceValid) return;
                MonoSingleton<MysteriousStrangerMgr>.Instance.ForceAppearMysteriousStranger();
                Logger.LogInfo("AutoStranger: stranger summoned (test key)");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
            }
        }

        private void Catch()
        {
            if (Time.unscaledTime < _cooldownUntil) return;
            if (!MonoSingleton<MysteriousStrangerMgr>.IsInstanceValid) return;

            MysteriousStrangerMgr mgr = MonoSingleton<MysteriousStrangerMgr>.Instance;
            MysteriousStranger stranger = mgr.CurrentMysteriousStranger;
            if (stranger == null) return;

            object tapped = ReadMember(stranger, "AlreadyTapped");
            if (tapped is bool && (bool)tapped) return;

            Room room = ReadMember(stranger, "CurrentRoom") as Room;
            if (room == null)
            {
                if (!_warnedMissingRoom)
                {
                    _warnedMissingRoom = true;
                    Logger.LogWarning("AutoStranger: stranger has no CurrentRoom, nothing to tap yet");
                }
                return;
            }

            if (mgr.CheckAndTapMysteriousStranger(room))
            {
                _cooldownUntil = Time.unscaledTime + _cooldown.Value;
                Logger.LogInfo("AutoStranger: Mysterious Stranger caught");
            }
        }
    }
}
