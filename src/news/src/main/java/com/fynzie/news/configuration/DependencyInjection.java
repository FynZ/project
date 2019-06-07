package com.fynzie.news.configuration;

import com.github.slugify.Slugify;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import springfox.documentation.builders.PathSelectors;
import springfox.documentation.builders.RequestHandlerSelectors;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spring.web.plugins.Docket;

/**
 * DependencyInjection
 */
@Configuration
public class DependencyInjection {

    @Bean
    public Slugify slugify()
    {
        return new Slugify();
    }

    @Bean
    public Docket swagger()
    {
        return new Docket(DocumentationType.SWAGGER_2)
                .select()
                .apis(RequestHandlerSelectors.any())
                .paths(PathSelectors.any())
                .build();
    }
}