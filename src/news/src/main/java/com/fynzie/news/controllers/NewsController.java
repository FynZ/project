package com.fynzie.news.controllers;

import java.util.List;

import com.fynzie.news.dto.DetailedNews;
import com.fynzie.news.dto.NewsCreation;
import com.fynzie.news.dto.NewsSummary;
import com.fynzie.news.services.NewsService;
import com.fynzie.news.viewmodels.NewsViewModel;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

/**
 * NewsController
 */
@RestController
@RequestMapping("/news")
public class NewsController
{
    private final NewsService newsService;

    public NewsController(NewsService newsService)
    {
        this.newsService = newsService;
    }
    
    @GetMapping(value = {"/"}, produces = "application/json")
    @ResponseBody
    public List<NewsSummary> getNews()
    {
        return newsService.getNews();
    }

    @GetMapping(value = {"/{page}"}, produces = "application/json")
    @ResponseBody
    public List<NewsSummary> getNews(@PathVariable int page)
    {
        return newsService.getByOffset(page);
    }

    @GetMapping(value = {"/detail/{news}"}, produces = "application/json")
    @ResponseBody
    public DetailedNews getDetailedNews(@PathVariable String slug)
    {
        return newsService.getBySlug(slug);
    }

    @PostMapping(value = {"/create"})
    public ResponseEntity<Object> createNews(@RequestBody NewsViewModel newsViewModel, BindingResult bindingResult)
    {
        if (!bindingResult.hasErrors())
        {
            NewsCreation newsCration = new NewsCreation(newsViewModel.getTitle(), newsViewModel.getContent(), -1);
            newsService.createNews(newsCration);

            return new ResponseEntity<>(HttpStatus.CREATED);
        }
        
        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}