package com.fynzie.gateway.config;

import java.io.IOException;
import java.security.SignatureException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Base64;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fynzie.gateway.models.JwtToken;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.web.filter.OncePerRequestFilter;

public class JwtAuthenticationFilter extends OncePerRequestFilter {

    // @Autowired
    // private UserDetailsService userDetailsService;

    // @Autowired
    // private JwtTokenUtil jwtTokenUtil;

    @Override
    protected void doFilterInternal(HttpServletRequest req, HttpServletResponse res, FilterChain chain)
            throws IOException, ServletException {
        String header = req.getHeader("Authorization");

        String[] splittedToken = header.split(".");

        String alg = Base64.getDecoder().decode(splittedToken[0]).toString();
        String payload = Base64.getDecoder().decode(splittedToken[1]).toString();
        String signature = Base64.getDecoder().decode(splittedToken[2]).toString();

        // need to parse the jwt here to setup authorities and co
        // so let's say the decoding was ok and we are no manipulatitng a JwtToken
        JwtToken decodedToken = new JwtToken();

        List<SimpleGrantedAuthority> authorities = decodedToken.getRoles()
            .stream()
            .map(role -> new SimpleGrantedAuthority(role.startsWith("ROLE_") ? role : "ROLE_" + role))
            .collect(Collectors.toList());
        
        Authentication authentication =  new UsernamePasswordAuthenticationToken(decodedToken.getUsername(), null, authorities);
        SecurityContextHolder.getContext().setAuthentication(authentication);

        chain.doFilter(req, res);
    }

    // @Override
    // protected void doFilterInternal(HttpServletRequest req, HttpServletResponse res, FilterChain chain) throws IOException, ServletException {
    //     String header = req.getHeader("Authorization");
    //     String username = null;
    //     String authToken = null;
    //     if (header != null && header.startsWith("Bearer ")) {
    //         authToken = header.replace("Bearer ", "");
    //         try {
    //             username = jwtTokenUtil.getUsernameFromToken(authToken);
    //         } catch (IllegalArgumentException e) {
    //             logger.error("an error occured during getting username from token", e);
    //         } catch (ExpiredJwtException e) {
    //             logger.warn("the token is expired and not valid anymore", e);
    //         } catch(SignatureException e){
    //             logger.error("Authentication Failed. Username or Password not valid.");
    //         }
    //     } else {
    //         logger.warn("couldn't find bearer string, will ignore the header");
    //     }
    //     if (username != null && SecurityContextHolder.getContext().getAuthentication() == null) {

    //         UserDetails userDetails = userDetailsService.loadUserByUsername(username);

    //         if (jwtTokenUtil.validateToken(authToken, userDetails)) {
    //             UsernamePasswordAuthenticationToken authentication = new UsernamePasswordAuthenticationToken(userDetails, null, Arrays.asList(new SimpleGrantedAuthority("ROLE_ADMIN")));
    //             authentication.setDetails(new WebAuthenticationDetailsSource().buildDetails(req));
    //             logger.info("authenticated user " + username + ", setting security context");
    //             SecurityContextHolder.getContext().setAuthentication(authentication);
    //         }
    //     }

    //     chain.doFilter(req, res);
    // }
}