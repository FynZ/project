package com.fynzie.news.configuration;

import com.fynzie.news.configuration.SecurityConstants;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.ExpiredJwtException;
import io.jsonwebtoken.Jws;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.MalformedJwtException;
import io.jsonwebtoken.UnsupportedJwtException;
import io.jsonwebtoken.security.SignatureException;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

/**
 * JwtAuthorizationFilter
 */
public class JwtAuthorizationFilter extends BasicAuthenticationFilter
{
    private static final Logger log = LoggerFactory.getLogger(JwtAuthorizationFilter.class);

    public JwtAuthorizationFilter(AuthenticationManager authenticationManager)
    {
        super(authenticationManager);
    }

    @Override
    protected void doFilterInternal(
        HttpServletRequest request, 
        HttpServletResponse response,
        FilterChain filterChain)
            throws IOException, ServletException
    {
        UsernamePasswordAuthenticationToken authentication = getAuthentication(request);
        String header = request.getHeader(SecurityConstants.TOKEN_HEADER);

        if (StringUtils.isEmpty(header) || !header.startsWith(SecurityConstants.TOKEN_PREFIX))
        {
            filterChain.doFilter(request, response);
            return;
        }

        SecurityContextHolder.getContext().setAuthentication(authentication);
        filterChain.doFilter(request, response);
    }

    private UsernamePasswordAuthenticationToken getAuthentication(HttpServletRequest request)
    {
        String token = request.getHeader(SecurityConstants.TOKEN_HEADER);
        if (StringUtils.isNotEmpty(token))
        {
            try
            {
                byte[] signingKey = SecurityConstants.JWT_SECRET.getBytes();

                Jws<Claims>  parsedToken = Jwts.parser()
                    .setSigningKey(SecurityConstants.getPublicKey())
                    .parseClaimsJws(token.replace(SecurityConstants.TOKEN_PREFIX, ""));

                String username = parsedToken
                    .getBody()
                    .getSubject();

                Object obj = parsedToken.getBody().get(SecurityConstants.ROLE_KEY);

                List<SimpleGrantedAuthority> authorities = new ArrayList<SimpleGrantedAuthority>();
                if (obj instanceof String)
                {
                    String role = (String) obj;
                    authorities.add(new SimpleGrantedAuthority(SecurityConstants.SPRING_SECURITY_ROLE_PREFIX + ((String) role).toUpperCase()));
                }
                else if (obj instanceof List)
                {
                    List<?> roles = (List<?>) obj;

                    for(Object role : roles)
                    {
                        authorities.add(new SimpleGrantedAuthority(SecurityConstants.SPRING_SECURITY_ROLE_PREFIX + ((String) role).toUpperCase()));
                    }
                }

                // List<SimpleGrantedAuthority> authorities = ((List<?>) parsedToken.getBody()
                //     .get(SecurityConstants.ROLE_KEY)).stream()
                //     .map(authority -> {
                //         SimpleGrantedAuthority authorityBis = new SimpleGrantedAuthority((String) authority);
                //         return authorityBis;
                //     })
                //     .collect(Collectors.toList());

                if (StringUtils.isNotEmpty(username))
                {
                    return new UsernamePasswordAuthenticationToken(username, null, authorities);
                }
            }
            catch (ExpiredJwtException exception)
            {
                log.warn("Request to parse expired JWT : {} failed : {}", token, exception.getMessage());
            }
            catch (UnsupportedJwtException exception)
            {
                log.warn("Request to parse unsupported JWT : {} failed : {}", token, exception.getMessage());
            }
            catch (MalformedJwtException exception)
            {
                log.warn("Request to parse invalid JWT : {} failed : {}", token, exception.getMessage());
            }
            catch (SignatureException exception)
            {
                log.warn("Request to parse JWT with invalid signature : {} failed : {}", token, exception.getMessage());
            }
            catch (IllegalArgumentException exception)
            {
                log.warn("Request to parse empty or null JWT : {} failed : {}", token, exception.getMessage());
            }
        }

        return null;
    }
}