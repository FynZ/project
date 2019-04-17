package com.fynzie.gateway.security;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.fynzie.gateway.models.JwtAuthToken;

import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.web.authentication.AbstractAuthenticationProcessingFilter;

/**
 * JwtAuthFilter
 */
public class JwtAuthFilter extends AbstractAuthenticationProcessingFilter {
	
	public JwtAuthFilter() {
		super("/**");
	}
	
	@Override
    public Authentication attemptAuthentication(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse) throws AuthenticationException, IOException, ServletException {

        String header = httpServletRequest.getHeader("Authorization");

        List<SimpleGrantedAuthority> authorities = new ArrayList<SimpleGrantedAuthority>();
        // if (header == null || header.equals("")) {

        // }
        // else {

        // }

        JwtAuthToken token = new JwtAuthToken(header.substring(6));
        return getAuthenticationManager().authenticate(token);

        // authorities.add(new SimpleGrantedAuthority("ROLE_USER"));
        // authorities.add(new SimpleGrantedAuthority("ROLE_ADMIN"));


        // // if (header == null || !header.startsWith("Check ")) {
        // //     throw new RuntimeException("Token is missing");
        // // }

        // // String authenticationToken = header.substring(6);
        // // JwtAuthToken token = new JwtAuthToken(authenticationToken);

        // Authentication authentication =  new UsernamePasswordAuthenticationToken("test", null, authorities);
        // // SecurityContextHolder.getContext().setAuthentication(authentication);
        
        // return getAuthenticationManager().authenticate(authentication);
    }
}