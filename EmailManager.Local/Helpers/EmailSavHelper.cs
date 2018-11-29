using Yellow.ApplicationServices.Infrastructure.IoC;

namespace Yellow.ApplicationServices.Infrastructure.Mail.Helpers
{
    public class EmailSavHelper
    {
        private readonly string _contentImagesEmailPath;
        private string _inviteToNoddleSavDeclinesEmailContentPath;
        private string _inviteToNoddleSavCardHoldersEmailContentPath;
        private string _inviteToNoddleExistingSavDeclinesEmailContentPath;
        private string _inviteToNoddleExistingSavCardHoldersEmailContentPath;
        private string _chaseToSignUpInNoddleSavNewCustomerEmailContentPath;
        private string _chaseToSignUpInNoddleSavDeclinedUserEmailContentPath;

        public EmailSavHelper(string contentImagesEmailPath)
        {
            _contentImagesEmailPath = contentImagesEmailPath;
        }

        public string GetInviteToNoddleSavDeclinesAssetPath(string asset)
        {
            if (_inviteToNoddleSavDeclinesEmailContentPath != null)
                return _inviteToNoddleSavDeclinesEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _inviteToNoddleSavDeclinesEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath + "InvSDU_v1/";

            return _inviteToNoddleSavDeclinesEmailContentPath + asset;
        }

        public string GetInviteToNoddleSavCardHoldersAssetPath(string asset)
        {
            if (_inviteToNoddleSavCardHoldersEmailContentPath != null)
                return _inviteToNoddleSavCardHoldersEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _inviteToNoddleSavCardHoldersEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath + "InvSCH_v1/";

            return _inviteToNoddleSavCardHoldersEmailContentPath + asset;
        }

        public string GetInviteToNoddleExistingSavDeclinesAssetPath(string asset)
        {
            if (_inviteToNoddleExistingSavDeclinesEmailContentPath != null)
                return _inviteToNoddleExistingSavDeclinesEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _inviteToNoddleExistingSavDeclinesEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath + "InvExstSDU_V1/";

            return _inviteToNoddleExistingSavDeclinesEmailContentPath + asset;
        }

        public string GetInviteToNoddleExistingSavCardHoldersAssetPath(string asset)
        {
            if (_inviteToNoddleExistingSavCardHoldersEmailContentPath != null)
                return _inviteToNoddleExistingSavCardHoldersEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _inviteToNoddleExistingSavCardHoldersEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath +
                                                                    "InvExstSCH_V1/";

            return _inviteToNoddleExistingSavCardHoldersEmailContentPath + asset;
        }

        public string GetChaseToSignUpInNoddleSavNewCustomerAssetPath(string asset)
        {
            if (_chaseToSignUpInNoddleSavNewCustomerEmailContentPath != null)
                return _chaseToSignUpInNoddleSavNewCustomerEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _chaseToSignUpInNoddleSavNewCustomerEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath + "ChaseSNC_v1/";

            return _chaseToSignUpInNoddleSavNewCustomerEmailContentPath + asset;
        }

        public string GetChaseToSignUpInNoddleSavDeclinedUserAssetPath(string asset)
        {
            if (_chaseToSignUpInNoddleSavDeclinedUserEmailContentPath != null)
                return _chaseToSignUpInNoddleSavDeclinedUserEmailContentPath + asset;

            var appSettings = ServiceLocator.GetInstance<IAppSettings>();
            _chaseToSignUpInNoddleSavDeclinedUserEmailContentPath = appSettings.NoddleDomainName + _contentImagesEmailPath + "ChaseSDU_v1/";

            return _chaseToSignUpInNoddleSavDeclinedUserEmailContentPath + asset;
        }
    }
}