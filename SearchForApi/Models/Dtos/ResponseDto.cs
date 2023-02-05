namespace SearchForApi.Models.Dtos
{
    public enum ResponseStatusType
    {
        OK = 200,
        ERROR = -1
    }

    public class ResponseDto<T>
    {
        public ResponseDto()
        {
            Status = ResponseStatusType.OK;
        }

        public ResponseDto(T result)
        {
            Status = ResponseStatusType.OK;
            Result = result;
        }

        public ResponseStatusType Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class Null { }
}

