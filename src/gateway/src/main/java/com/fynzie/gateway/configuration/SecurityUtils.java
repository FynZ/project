package com.fynzie.gateway.configuration;

import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.PublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

import io.jsonwebtoken.io.IOException;

/**
 * SecurityUtils
 */
public final class SecurityUtils
{
    private SecurityUtils()
    {
        throw new IllegalStateException("Cannot create instance of static util class");
    }

    public static PublicKey getPublicKey()
    {
        String stripped = stripBeginEnd(SecurityConstants.PUBLIC_KEY);
        byte[] bytes = pemToDer(stripped);

        try
        {
            return decodePublicKey(bytes);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static String stripBeginEnd(String pem)
    {
        String stripped = pem.replaceAll("-----BEGIN PUBLIC KEY-----", "");
        stripped = stripped.replaceAll("-----END PUBLIC KEY-----", "");
        stripped = stripped.replaceAll("\r\n", "");
        stripped = stripped.replaceAll("\n", "");

        return stripped.trim();
    }

    public static byte[] pemToDer(String pem) throws IOException
    {
        return Base64.getDecoder().decode(stripBeginEnd(pem));
    }

    public static PublicKey decodePublicKey(byte[] der) 
        throws NoSuchAlgorithmException, InvalidKeySpecException, NoSuchProviderException
    {
        X509EncodedKeySpec spec = new X509EncodedKeySpec(der);

        KeyFactory kf = KeyFactory.getInstance("RSA"
                //        , "BC" //use provider BouncyCastle if available.
        );

        return kf.generatePublic(spec);
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
}