#ifndef PRODUCERCONSUMER_H
#define PRODUCERCONSUMER_H


#include <queue>
#include <boost/function.hpp>
#include <boost/thread/locks.hpp>
#include <boost/thread/mutex.hpp>
#include <boost/thread/condition.hpp>
#include <boost/thread/thread.hpp>

template <class C, typename T>
class ProducerConsumer
{
    public:
    typedef boost::function<void(C*, T)> Consume;

    bool empty()
    {
        boost::unique_lock<boost::mutex> mlock(mutex_);
        bool empty = queue_.empty();
        mlock.unlock();
        return empty;
    }

    T pop(bool& success)
    {
        boost::unique_lock<boost::mutex> mlock(mutex_);
        while (!stopExecution && queue_.empty())
        {
            cond_.wait(mlock);
        }
        if (stopExecution && queue_.empty())
        {
            success = false;
            return T();
        }
        T item = queue_.front();
        queue_.pop();
        mlock.unlock();
        success = true;
        return item;
    }

    bool pop(T& item)
    {
        boost::unique_lock<boost::mutex> mlock(mutex_);
        while (queue_.empty())
        {
            cond_.wait(mlock);
        }
        if (stopExecution && queue_.empty())
        {
            return false;
        }
        item = queue_.pop();
        queue_.pop();
        mlock.unlock();
        return true;
    }

    void push(const T& item)
    {
        if (stopExecution == true)
            return;
        boost::unique_lock<boost::mutex> mlock(mutex_);
        queue_.push(item);
        mlock.unlock();
        cond_.notify_one();
    }

    void push(T&& item)
    {
        if (stopExecution == true)
            return;
        boost::unique_lock<boost::mutex> mlock(mutex_);
        queue_.push(std::move(item));
        mlock.unlock();
        cond_.notify_one();
    }

    void stop()
    {
        boost::unique_lock<boost::mutex> mlock(mutex_);
        stopExecution = true;
        mlock.unlock();
        cond_.notify_one();
        consumerThread.join();
    }

    void start()
    {
        consumerThread = boost::thread(&ProducerConsumer::execute, this);
    }

    void setClassObject(C* obj)
    {
        classObject = obj;
    }

    Consume ConsumeHandler;

    private:
    C* classObject;
    std::queue<T> queue_;
    boost::mutex mutex_;
    boost::condition_variable cond_;
    boost::thread consumerThread;
    bool stopExecution = false;


    void execute()
    {
        bool running = true;
        while (running)
        {
            T item = pop(running);
            if (item)
                ConsumeHandler(classObject, item);

        }
    }

};

#endif