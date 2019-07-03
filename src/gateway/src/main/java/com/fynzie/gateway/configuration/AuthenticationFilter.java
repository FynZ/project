package com.fynzie.gateway.configuration;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang3.StringUtils;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import io.jsonwebtoken.Jwts;

/**
 * AuthenticationFilter
 */
@Component
@Order(1)
public class AuthenticationFilter implements Filter
{

    @Override
    public void init(FilterConfig filterConfig) throws ServletException {

    }

    @Override
    public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain)
            throws IOException, ServletException
    {
        HttpServletRequest httpRequest = (HttpServletRequest)request;

        String token = httpRequest.getHeader(SecurityConstants.TOKEN_HEADER);
        // if a token is present, verify signature
        if (StringUtils.isNotEmpty(token))
        {
            try
            {
                Jwts.parser()
                    .setSigningKey(SecurityUtils.getPublicKey())
                    .parse(token.replace(SecurityConstants.TOKEN_PREFIX, ""));
                    // .parseClaimsJws(token.replace(SecurityConstants.TOKEN_PREFIX, ""));

                // successfully parsed token, carry on
                chain.doFilter(request, response);
            }
            catch(Exception e)
            {
                ((HttpServletResponse) response).sendError(HttpServletResponse.SC_UNAUTHORIZED, "Invalid token signature");
            }
        }
        
        // not an httprequest or no token, carry on
        chain.doFilter(request, response);
    }

    @Override
    public void destroy() {

    }

}