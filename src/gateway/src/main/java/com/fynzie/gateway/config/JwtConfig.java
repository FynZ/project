package com.fynzie.gateway.config;

import java.util.Collections;

import com.fynzie.gateway.security.JwtAuthFilter;
import com.fynzie.gateway.security.JwtAuthProvider;
import com.fynzie.gateway.security.JwtAuthenticationEntryPoint;
import com.fynzie.gateway.security.JwtSuccessHandler;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.ProviderManager;
import org.springframework.security.config.annotation.method.configuration.EnableGlobalMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@EnableGlobalMethodSecurity(prePostEnabled=true)
@EnableWebSecurity
@Configuration
public class JwtConfig extends WebSecurityConfigurerAdapter{
	
	@Autowired
	private JwtAuthProvider autheticationProvider;
	
	@Autowired
	private JwtAuthenticationEntryPoint entryPoint;

	@Bean
	public AuthenticationManager authenticationManager() {
		return new ProviderManager(Collections.singletonList(autheticationProvider));
	}
	
	//create a custom filter
	// @Bean
	// public JwtAuthFilter authTokenFilter() {
		
	// 	JwtAuthFilter filter = new JwtAuthFilter();
	// 	filter.setAuthenticationManager(authenticationManager());
	// 	filter.setAuthenticationSuccessHandler(new JwtSuccessHandler());
		
	// 	return filter;
	// }

	@Override
	protected void configure(HttpSecurity http) throws Exception {
		
		http.csrf()
			.disable()
			.authorizeRequests()
				.antMatchers("/news/**")
					.permitAll()
				.antMatchers("/ressources/**")
					.permitAll()
				.antMatchers("/auth/**")
					.permitAll()
				.antMatchers("/monsters/**")
					.permitAll()
				.antMatchers("/trading/**")
					.permitAll()
				.antMatchers("/marketplace/**")
					.permitAll()
				.antMatchers("/map/**")
					.permitAll()
				.antMatchers("/messaging/**")
					.permitAll()
				.antMatchers("/**")
					.permitAll()
				.and()
					.exceptionHandling()
						.authenticationEntryPoint(entryPoint)
				.and()
					.sessionManagement()
						.sessionCreationPolicy(SessionCreationPolicy.STATELESS);
		
		// http.addFilterBefore(authTokenFilter(), UsernamePasswordAuthenticationFilter.class);
	
		http.headers().cacheControl();
	}
}