using AlumniProject.ExceptionHandler;
using System.Security.Claims;

namespace AlumniProject.Ultils
{
    public class TokenUltil
    {
        public TokenUltil()
        {

        }
        public Claim GetClaimByType(ClaimsPrincipal user,string type)
        {
            var value = user.Claims.FirstOrDefault(c => c.Type == type);
            if(value == null)
            {
                throw new BadRequestException("Token is not valid");
            }
            return value;
        }
    }
}
