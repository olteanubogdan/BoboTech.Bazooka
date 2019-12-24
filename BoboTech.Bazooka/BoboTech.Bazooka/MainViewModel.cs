using DevExpress.Mvvm;
using GlobalHotKey;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;

namespace BoboTech.Bazooka
{
    public class MainViewModel : ViewModelBase
    {
        //private IMessageBoxService MessageBoxService => GetService<IMessageBoxService>(); //note to self: DX MVVM services are not available yet in ctor

        private HotKeyManager _bazookaHotKeyManager;

        private HotKeyManager _starPactHotKeyManager;

        private HotKey _bazookaHotKey;

        private HotKey _starPactHotKey;

        private VirtualKeyCode _generatorKey;

        private VirtualKeyCode _buffKey;

        private VirtualKeyCode _meteorKey;

        private VirtualKeyCode _archonKey;

        private int _starPactWaitTime;

        private int _startChannelWaitTime;

        private int _buffWaitTime;

        public virtual string WindowTitle { get; set; } = "Diablo 3 bazooka macro manager";

        public virtual List<ListItem> VirtualKeyCodeList { get; set; }

        public virtual List<ListItem> HotKeyList { get; set; }

        public virtual ListItem GeneratorKey { get; set; }

        public virtual ListItem BuffKey { get; set; }

        public virtual ListItem MeteorKey { get; set; }

        public virtual ListItem ArchonKey { get; set; }

        public virtual ListItem BazookaMacroKey { get; set; }

        public virtual ListItem StarPactMacroKey { get; set; }

        public virtual int StarPactWaitTime { get; set; }

        public virtual int StartChannelWaitTime { get; set; }

        public virtual int BuffWaitTime { get; set; }

        public void ViewLoaded()
        {
            VirtualKeyCodeList = ((VirtualKeyCode[])Enum.GetValues(typeof(VirtualKeyCode))).Select(x => new ListItem { Value = (int)x, Text = x.ToString() }).ToList();
            HotKeyList = ((Key[])Enum.GetValues(typeof(Key))).Select(x => new ListItem { Value = (int)x, Text = x.ToString() }).ToList();

            VirtualKeyCode virtualKeyCode = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), ConfigurationManager.AppSettings[nameof(GeneratorKey)]);
            GeneratorKey = VirtualKeyCodeList.FirstOrDefault(x => x.Value == (int)virtualKeyCode);

            virtualKeyCode = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), ConfigurationManager.AppSettings[nameof(BuffKey)]);
            BuffKey = VirtualKeyCodeList.FirstOrDefault(x => x.Value == (int)virtualKeyCode);

            virtualKeyCode = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), ConfigurationManager.AppSettings[nameof(MeteorKey)]);
            MeteorKey = VirtualKeyCodeList.FirstOrDefault(x => x.Value == (int)virtualKeyCode);

            virtualKeyCode = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), ConfigurationManager.AppSettings[nameof(ArchonKey)]);
            ArchonKey = VirtualKeyCodeList.FirstOrDefault(x => x.Value == (int)virtualKeyCode);

            Key key = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings[nameof(BazookaMacroKey)]);
            BazookaMacroKey = HotKeyList.FirstOrDefault(x => x.Value == (int)key);

            key = (Key)Enum.Parse(typeof(Key), ConfigurationManager.AppSettings[nameof(StarPactMacroKey)]);
            StarPactMacroKey = HotKeyList.FirstOrDefault(x => x.Value == (int)key);

            StarPactWaitTime = int.Parse(ConfigurationManager.AppSettings[nameof(StarPactWaitTime)]);
            StartChannelWaitTime = int.Parse(ConfigurationManager.AppSettings[nameof(StartChannelWaitTime)]);
            BuffWaitTime = int.Parse(ConfigurationManager.AppSettings[nameof(BuffWaitTime)]);
        }

        public bool CanStartBazookaMacro() => _bazookaHotKeyManager == null;

        public void StartBazookaMacro()
        {
            _bazookaHotKeyManager = new HotKeyManager();
            _bazookaHotKey = _bazookaHotKeyManager.Register((Key)BazookaMacroKey.Value, ModifierKeys.None);
            _meteorKey = (VirtualKeyCode)MeteorKey.Value;
            _buffKey = (VirtualKeyCode)BuffKey.Value;
            _archonKey = (VirtualKeyCode)ArchonKey.Value;
            _starPactWaitTime = StarPactWaitTime;
            _startChannelWaitTime = StartChannelWaitTime;
            _buffWaitTime = BuffWaitTime;
            _bazookaHotKeyManager.KeyPressed += BazookaHotKeyManagerKeyPressed;
        }

        private void BazookaHotKeyManagerKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.HotKey.Key != _bazookaHotKey.Key)
                return;

            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyPress(_meteorKey);
            inputSimulator.Keyboard.KeyPress(_meteorKey);
            inputSimulator.Keyboard.KeyPress(_meteorKey);
            Thread.Sleep(_starPactWaitTime);
            inputSimulator.Keyboard.KeyDown(_buffKey);
            Thread.Sleep(_startChannelWaitTime);
            inputSimulator.Keyboard.KeyPress(_archonKey);
            Thread.Sleep(_buffWaitTime);
            inputSimulator.Keyboard.KeyUp(_buffKey);
        }

        public bool CanStopBazookaMacro() => _bazookaHotKeyManager != null;

        public void StopBazookaMacro()
        {
            _bazookaHotKeyManager.Unregister(_bazookaHotKey);
            _bazookaHotKeyManager.Dispose();
            _bazookaHotKeyManager = null;
        }

        public bool CanStartStarPactMacro() => _starPactHotKeyManager == null;

        public void StartStarPactMacro()
        {
            _starPactHotKeyManager = new HotKeyManager();
            _starPactHotKey = _starPactHotKeyManager.Register((Key)StarPactMacroKey.Value, ModifierKeys.None);
            _meteorKey = (VirtualKeyCode)MeteorKey.Value;
            _generatorKey = (VirtualKeyCode)GeneratorKey.Value;
            _buffKey = (VirtualKeyCode)BuffKey.Value;
            _starPactWaitTime = StarPactWaitTime;
            _buffWaitTime = BuffWaitTime;
            _starPactHotKeyManager.KeyPressed += StarPactHotKeyManagerKeyPressed;
        }

        private void StarPactHotKeyManagerKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.HotKey.Key != _starPactHotKey.Key)
                return;

            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyPress(_meteorKey);
            inputSimulator.Keyboard.KeyPress(_meteorKey);
            inputSimulator.Keyboard.KeyPress(_meteorKey);

            switch(_generatorKey)
            {
                case VirtualKeyCode.LBUTTON:
                    inputSimulator.Mouse.LeftButtonDown();
                    break;
                case VirtualKeyCode.RBUTTON:
                    inputSimulator.Mouse.RightButtonDown();
                    break;
                default:
                    inputSimulator.Keyboard.KeyDown(_generatorKey);
                    break;
            }

            Thread.Sleep(_starPactWaitTime);

            switch (_generatorKey)
            {
                case VirtualKeyCode.LBUTTON:
                    inputSimulator.Mouse.LeftButtonUp();
                    break;
                case VirtualKeyCode.RBUTTON:
                    inputSimulator.Mouse.RightButtonUp();
                    break;
                default:
                    inputSimulator.Keyboard.KeyUp(_generatorKey);
                    break;
            }

            inputSimulator.Keyboard.KeyDown(_buffKey);
            Thread.Sleep(_buffWaitTime);
            inputSimulator.Keyboard.KeyUp(_buffKey);
        }

        public bool CanStopStarPactMacro() => _starPactHotKeyManager != null;

        public void StopStarPactMacro()
        {
            _starPactHotKeyManager.Unregister(_starPactHotKey);
            _starPactHotKeyManager.Dispose();
            _starPactHotKeyManager = null;
        }
    }

    public class ListItem
    {
        public int Value { get; set; }

        public string Text { get; set; }
    }
}