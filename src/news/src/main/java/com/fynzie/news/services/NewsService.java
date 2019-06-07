package com.fynzie.news.services;

import java.util.List;

import com.fynzie.news.dto.DetailedNews;
import com.fynzie.news.dto.NewsCreation;
import com.fynzie.news.dto.NewsSummary;

/**
 * NewsService
 */
public interface NewsService
{
    boolean createNews(NewsCreation newsCreation);

    public DetailedNews getById(long id);

    public DetailedNews getBySlug(String slug);

    public List<NewsSummary> getNews();

    public List<NewsSummary> getByOffset(int offset);
}