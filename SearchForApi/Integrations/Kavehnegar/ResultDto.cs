using System;
namespace SearchForApi.Integrations.Kavehnegar
{
    public class ResultDto<T>
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}

