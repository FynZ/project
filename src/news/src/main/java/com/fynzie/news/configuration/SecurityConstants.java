package com.fynzie.news.configuration;

import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.PublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

import io.jsonwebtoken.io.IOException;

/**
 * SecurityConstants
 */
public final class SecurityConstants
{
    public static final String AUTH_LOGIN_URL = "/api/authenticate";

    // Signing key for HS512 algorithm
    // You can use the page http://www.allkeysgenerator.com/ to generate all kinds of keys
    public static final String JWT_SECRET = "n2r5u8x/A%D*G-KaPdSgVkYp3s6v9y$B&E(H+MbQeThWmZq4t7w!z%C*F-J@NcRf";
    //public static final String JWT_PUB = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQDbVLwRChfczBYFzNq1dbCtiaDKA6siFrgE0bdEMQ7/tn08bgzAJumpiXdCSP8Dx5q706ZMkYfA3MgRkQGpF5LCxGmu+U7YDhQEtPbwwlyoDcwW3RA9L3Sa3mN35ZYM3fP5eLjugspZIiKtcDrVobAIDOEenW/VcGGr2QLo9pqLX5QIEGZNLiOvg9Ar9j7HYgPK8i2Nn5TSrKccZJWUSqTRf46AVvfIoaS6ToViq1Xix8zAL1gt6dBD4QqpXL9w+cH8CfBMXtatGDDJar9Ujq5BAoRr+NyhwbEknqpaarYrLcGcqDcK1hkTwcsZPNyMjwhriK6TOpSbIz59AKJvo7gaxMElSfQdQIqO+8idvWRKvk+NhgvWqwW2EpnceTGtmJh+Akc3+30962AX5ECjAylZmX1+rMxi1thAqdtesX7kXYjpJ98yq8EwBr23QhNFYhF5kXFMCOcPwrhemFWMEY/Fspc/aorgXysT7xmdk5/aUoXzfixmFxGcJp5PsNV89dDfKsv0gbmOUYJKI8ABGTvpKxoHbO+lZrrch27cuo7Sj8dGF8JnIz/14Yzm1J2KMLFSKDqOLyTp6LTc0VyP9cBFlN6UxdB8KFKW8bP0r34lNbptpd9NXxJkMEtZdXrtu5Ow2+44iId03FkeM/QQ8aUwIIG5A6f75SXlkTqes2lleQ== fynzie@FYNZIE-DESKTO";
    public static final String PUBLIC_KEY = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAspFFSYQY6Msghqw6SdeK3dgkJMfB2XVmUeI7hoji0fMwRLF40nBRo4WKLE+r9msE2urVuyklumN+fI28zUuX993zZ2bNjFnvQKhH1FnPYyAo0D+HF89hHy1gMvmcWBD6exvnUelNQVT3PSpF0fpMPLONfc9+jAIPLb0PGpcNpLw2Y5cEP+M/mRZYGqYcA6IfhLxcKXnsjS3zynDQev5G/YJ+4AraIeTO6w+RX2oXy6Ik9zjVYoiWzkDuIdvrq+lN2/UT91W5f37f9Fikx5cfvkX+gQafkJZk50YvD6c3W20kmOaw/qW2G3QeM5Bo+uAHHQmjj7wCld28B9HZVFmPeQIDAQAB-----END PUBLIC KEY-----";

    // JWT token defaults
    public static final String TOKEN_HEADER = "Authorization";
    public static final String TOKEN_PREFIX = "Bearer ";
    public static final String TOKEN_TYPE = "JWT";
    public static final String TOKEN_ISSUER = "secure-api";
    public static final String TOKEN_AUDIENCE = "secure-app";
    public static final String ROLE_KEY = "roles";
    public static final String SPRING_SECURITY_ROLE_PREFIX = "ROLE_";

    private SecurityConstants()
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
}