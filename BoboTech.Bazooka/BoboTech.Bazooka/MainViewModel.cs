using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace BoboTech.Bazooka
{
    public class MainViewModel : ViewModelBase
    {
        private IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

        public virtual string WindowTitle { get; set; } = "Diablo 3 bazooka macro manager";

        public virtual List<ListItem> StarPactMeteorKeyValues { get; set; }

        public virtual ListItem SelectedStarPactMeteorKey { get; set; }

        public async Task TestAsync()
        {
            await Task.Delay(1000);
            MessageBoxService.ShowMessage("Waited 1000 ms.", WindowTitle, MessageButton.OK, MessageIcon.Information);
        }

        public void ViewLoaded()
        {
            StarPactMeteorKeyValues = ((VirtualKeyCode[])Enum.GetValues(typeof(VirtualKeyCode))).Select(x => new ListItem { Value = (int)x, Text = x.ToString() }).ToList();
        }
    }

    public class ListItem
    {
        public int Value { get; set; }

        public string Text { get; set; }
    }
}