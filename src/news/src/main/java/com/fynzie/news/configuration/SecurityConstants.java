package com.fynzie.news.configuration;

/**
 * SecurityConstants
 */
public final class SecurityConstants
{
    public static final String AUTH_LOGIN_URL = "/api/authenticate";

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
}