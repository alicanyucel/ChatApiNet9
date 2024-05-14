namespace ChatApi.Dtos
{
    public sealed record SendeMessageDto(Guid UserId,Guid ToUserId,string mesage);
   
}
