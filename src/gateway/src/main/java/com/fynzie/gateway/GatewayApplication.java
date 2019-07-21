package com.fynzie.gateway;

import java.util.Arrays;
import java.util.Collections;
import java.util.stream.Collectors;
import org.springframework.http.HttpMethod;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;
import org.springframework.web.filter.CorsFilter;

import org.springframework.cloud.netflix.zuul.EnableZuulProxy;

@ComponentScan("com.fynzie")
@Configuration
@EnableZuulProxy
@SpringBootApplication
public class GatewayApplication
{
    @Bean
    public CorsFilter corsFilter()
    {
        // final CorsConfiguration config = new CorsConfiguration();
        // config.setAllowCredentials(true);
        // config.addAllowedOrigin("*");
        // config.addAllowedHeader("*");
        // config.addAllowedMethod("OPTIONS");
        // config.addAllowedMethod("HEAD");
        // config.addAllowedMethod("GET");
        // config.addAllowedMethod("PUT");
        // config.addAllowedMethod("POST");
        // config.addAllowedMethod("DELETE");
        // config.addAllowedMethod("PATCH");

        // final UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        // source.registerCorsConfiguration("/**", config);

        // return new CorsFilter(source);

        final UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        final CorsConfiguration config = new CorsConfiguration();
        config.setAllowCredentials(true);
        config.setAllowedOrigins(Collections.singletonList("*"));
        config.setAllowedHeaders(Collections.singletonList("*"));
        config.setAllowedMethods(Arrays.stream(HttpMethod.values()).map(HttpMethod::name).collect(Collectors.toList()));
        source.registerCorsConfiguration("/**", config);
        return new CorsFilter(source);
    }

    public static void main(String[] args)
    {
		SpringApplication.run(GatewayApplication.class, args);
	}
}
