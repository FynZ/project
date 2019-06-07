package com.fynzie.news.services;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

import com.fynzie.news.dto.DetailedNews;
import com.fynzie.news.dto.NewsCreation;
import com.fynzie.news.dto.NewsSummary;
import com.fynzie.news.models.News;
import com.fynzie.news.repositories.NewsRepository;
import com.github.slugify.Slugify;

import org.springframework.stereotype.Service;

/**
 * NewsServiceImpl
 */
@Service
public class NewsServiceImpl implements NewsService {

    private final NewsRepository newsRepository;
    private final Slugify slugify;

    public NewsServiceImpl(NewsRepository newsRepository, Slugify slugify) {
        this.newsRepository = newsRepository;
        this.slugify = slugify;
    }

    @Override
    public boolean createNews(NewsCreation newsCreation)
    {
        News news = new News(newsCreation.getTitle(), newsCreation.getContent(), newsCreation.getUserCreationId());

        news.setCreationDate(LocalDateTime.now());
        news.setModificationDate(LocalDateTime.now());
        news.setSlug(slugify.slugify(news.getTitle()));

        newsRepository.save(news);

        return true;
    }

    @Override
    public DetailedNews getById(long id)
    {
        return detailedTransform(newsRepository.findById(id));
    }

    @Override
    public DetailedNews getBySlug(String slug)
    {
        return detailedTransform(newsRepository.findBySlug(slug));
    }

    @Override
    public List<NewsSummary> getNews()
    {
        List<News> news = newsRepository.findAll();
        news.sort((a, b) -> {
            return a.getCreationDate().isAfter(b.getCreationDate()) ? -1 : 1;
        });

        if (news.size() <= 5)
        {
            return transform(news);
        }

        return transform(news.subList(0, 5));
    }

    @Override
    public List<NewsSummary> getByOffset(int offset)
    {
        if (offset <= 1)
        {
            return getNews();
        }

        List<News> news = newsRepository.findAll();
        news.sort((a, b) -> {
            return a.getCreationDate().isAfter(b.getCreationDate()) ? -1 : 1;
        });

        if (news.size() <= offset * 5)
        {
            return transform(news.subList(5 * (offset - 1), (5 * (offset - 1)) + news.size() % 5));
        }

        return transform(news.subList(5 * (offset - 1), (5 * (offset - 1)) + 5));
    }

    private static List<NewsSummary> transform(List<News> news)
    {
        return news.stream().map(x -> transform(x)).collect(Collectors.toList());
    }

    private static NewsSummary transform(News news)
    {
        if (news == null)
        {
            return null;
        }

        NewsSummary newsSummary = new NewsSummary();
        newsSummary.setTitle(news.getTitle());
        newsSummary.setSlug(news.getSlug());
        newsSummary.setContent(news.getContent());
        newsSummary.setUserCreationId(news.getUserCreationId());
        newsSummary.setCreationDate(news.getCreationDate());
        newsSummary.setModificationDate(news.getModificationDate());

        return newsSummary;
    }

    private static DetailedNews detailedTransform(News news)
    {
        DetailedNews detailedNews = new DetailedNews();
        detailedNews.setId(news.getId());
        detailedNews.setTitle(news.getTitle());
        detailedNews.setSlug(news.getSlug());
        detailedNews.setContent(news.getContent());
        detailedNews.setUserCreationId(news.getUserCreationId());
        detailedNews.setCreationDate(news.getCreationDate());
        detailedNews.setModificationDate(news.getModificationDate());
        detailedNews.setComments(news.getComments());
        
        return detailedNews;
    }
}