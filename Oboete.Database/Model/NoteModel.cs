using Oboete.Database.Entity;

namespace Oboete.Database.Model
{
    public class NoteModel
    {
        public NoteEntity NoteEntity { get; private set; }

        public int NoteId { get => NoteEntity.NoteId; }

        public NoteModel(NoteEntity noteEntity) => NoteEntity = noteEntity;
    }
}