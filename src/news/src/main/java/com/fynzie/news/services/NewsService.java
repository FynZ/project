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

    DetailedNews getById(long id);

    DetailedNews getBySlug(String slug);

    List<NewsSummary> getNews();

    List<NewsSummary> getByOffset(int offset);
}