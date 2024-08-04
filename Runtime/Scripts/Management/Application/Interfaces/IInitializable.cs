using System;

namespace Devenant
{
    public struct InitializationResponse
    {
        public readonly bool success;
        public readonly string error;

        public InitializationResponse(bool success, string error = "error")
        {
            this.success = success;
            this.error = error;
        }
    }

    public interface IInitializable
    {
        public void Initialize(Action<InitializationResponse> callback);
    }
}
