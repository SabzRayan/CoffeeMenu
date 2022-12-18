﻿namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetUsername();
        string GetUserId();
        string GetUserEmail();
    }
}