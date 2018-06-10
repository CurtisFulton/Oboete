using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    public abstract class BaseEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreateDateTime { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ModifiedDateTime { get; set; } = DateTimeOffset.UtcNow;


        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}