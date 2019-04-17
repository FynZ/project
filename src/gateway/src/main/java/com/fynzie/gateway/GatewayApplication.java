package com.fynzie.gateway;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
// import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.cloud.netflix.zuul.EnableZuulProxy;
import org.springframework.context.annotation.Bean;
// import org.springframework.security.oauth2.config.annotation.web.configuration.EnableResourceServer;
import org.springframework.context.annotation.ComponentScan;

import com.fynzie.gateway.config.JwtFilter;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

// @EnableResourceServer
@ComponentScan("com.fynzie")
@EnableZuulProxy
@SpringBootApplication
// @EnableDiscoveryClient
public class GatewayApplication {

	@Bean
	public FilterRegistrationBean jwtFilter() {
		final FilterRegistrationBean registrationBean = new FilterRegistrationBean();
		registrationBean.setFilter(new JwtFilter());

		return registrationBean;
	}

	public static void main(String[] args) {

        Logger log = LogManager.getLogger(GatewayApplication.class);
        log.info("Hello, World!");

		SpringApplication.run(GatewayApplication.class, args);
	}
}
