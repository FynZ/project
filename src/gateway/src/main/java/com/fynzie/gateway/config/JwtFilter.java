package com.fynzie.gateway.config;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.filter.GenericFilterBean;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureException;

public class JwtFilter extends GenericFilterBean {

	public void doFilter(final ServletRequest req, final ServletResponse res, final FilterChain chain)
			throws IOException, ServletException {

		final HttpServletRequest request = (HttpServletRequest) req;
		final HttpServletResponse response = (HttpServletResponse) res;
        final String authHeader = request.getHeader("authorization");
        
        List<SimpleGrantedAuthority> authorities = new ArrayList<SimpleGrantedAuthority>();
        authorities.add(new SimpleGrantedAuthority("ROLE_USER"));
        authorities.add(new SimpleGrantedAuthority("ROLE_ADMIN"));
        Authentication authentication =  new UsernamePasswordAuthenticationToken("test", null, authorities);
        SecurityContextHolder.getContext().setAuthentication(authentication);

		// if ("OPTIONS".equals(request.getMethod())) {
		// 	response.setStatus(HttpServletResponse.SC_OK);

		// 	chain.doFilter(req, res);
		// } else {

		// 	if (authHeader == null || !authHeader.startsWith("Bearer ")) {
		// 		throw new ServletException("Missing or invalid Authorization header");
		// 	}

		// 	final String token = authHeader.substring(7);

		// 	try {
		// 		final Claims claims = Jwts.parser().setSigningKey("secretkey").parseClaimsJws(token).getBody();
		// 		request.setAttribute("claims", claims);
		// 	} catch (final SignatureException e) {
		// 		throw new ServletException("Invalid token");
		// 	}

        chain.doFilter(req, res);
    }
}
