package com.fynzie.gateway;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
// import org.springframework.cloud.client.discovery.EnableDiscoveryClient;
import org.springframework.cloud.netflix.zuul.EnableZuulProxy;
import org.springframework.security.oauth2.config.annotation.web.configuration.EnableResourceServer;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

@EnableResourceServer
@EnableZuulProxy
@SpringBootApplication
// @EnableDiscoveryClient
public class GatewayApplication {

	public static void main(String[] args) {

        Logger log = LogManager.getLogger(GatewayApplication.class);
        log.info("Hello, World!");

		SpringApplication.run(GatewayApplication.class, args);
	}
}
