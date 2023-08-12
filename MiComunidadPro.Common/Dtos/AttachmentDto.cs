using MiComunidadPro.Common.Base;
using System.Runtime.Serialization;

namespace MiComunidadPro.Common.Dtos
{
    [DataContract]
    public class AttachmentDto : DTOBase<AttachmentDto>
    {
        #region Properties

        [DataMember]
        public int DocumentId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] Content { get; set; }

        #endregion
    }
}
