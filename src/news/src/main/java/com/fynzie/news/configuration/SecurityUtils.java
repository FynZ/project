package com.fynzie.news.configuration;

/**
 * SecurityUtils
 */
public final class SecurityUtils
{
    private SecurityUtils()
    {
        throw new IllegalStateException("Cannot create instance of static util class");
    }

    public static String removeTokenAuthenticationStrategy(String token)
    {
        if (token == null)
        {
            return "";
        }

        // strip only if at the beginning
        if (token.startsWith(SecurityConstants.TOKEN_PREFIX))
        {
            return token.replace(SecurityConstants.TOKEN_PREFIX, "");
        }

        return token;
    }

    public static String removeTokenSignature(String token)
    {
        if (token == null)
        {
            return "";
        }

        String[] splitted = token.split("\\.");

        if (splitted.length == 3)
        {
            return splitted[0] + "." + splitted[1] + ".";
        }

        return "";
    }
    
    public static String cleanToken(String token)
    {
        String stripped = removeTokenAuthenticationStrategy(token);
        String signatureLess = removeTokenSignature(stripped);
        
        return signatureLess;
    }
}