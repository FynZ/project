// package com.fynzie.gateway.config;

// // import com.auth0.jwt.JWT;
// // import com.auth0.jwt.algorithms.Algorithm;
// import org.springframework.security.authentication.AuthenticationManager;
// import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
// import org.springframework.security.core.context.SecurityContextHolder;
// import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;

// import javax.servlet.FilterChain;
// import javax.servlet.ServletException;
// import javax.servlet.http.HttpServletRequest;
// import javax.servlet.http.HttpServletResponse;
// import java.io.IOException;
// import java.util.ArrayList;

// /**
//  * JwtAuthorizationFilter
//  */
// public class JwtAuthorizationFilter extends BasicAuthenticationFilter {

//     public JwtAuthorizationFilter(AuthenticationManager authManager) {
//         super(authManager);
//       }
    
//       @Override
//       protected void doFilterInternal(HttpServletRequest req,
//                       HttpServletResponse res,
//                       FilterChain chain) throws IOException, ServletException {
//         String header = req.getHeader("Authorization");
    
//         if (header == null || !header.startsWith("Bearer ")) {
//           chain.doFilter(req, res);
//           return;
//         }
    
//         UsernamePasswordAuthenticationToken authentication = getAuthentication(req);
    
//         SecurityContextHolder.getContext().setAuthentication(authentication);
//         chain.doFilter(req, res);
//       }
    
//       private UsernamePasswordAuthenticationToken getAuthentication(HttpServletRequest request) {
//         String token = request.getHeader("Authorization");
//         if (token != null) {
//           //parse the token.
//         //   String user = JWT.require(Algorithm.HMAC512(SECRET.getBytes()))
//         //       .build()
//         //       .verify(token.replace("Bearer ", ""))
//         //       .getSubject();
    
//         //   if (user != null) {
//         //     return new UsernamePasswordAuthenticationToken(user, null, new ArrayList<>());
//         //   }
//           return null;
//         }
//         return null;
//       }
// }