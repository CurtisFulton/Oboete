using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oboete.Database.Entity
{
    /// <summary>
    /// Base type for any tables that have rows actively added/removed/updated
    /// </summary>
    public abstract class BaseEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset CreatedDateTime { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ModifiedDateTime { get; set; } = DateTimeOffset.UtcNow;
        
        public void OnPropertyChanged(string propertyName) => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}