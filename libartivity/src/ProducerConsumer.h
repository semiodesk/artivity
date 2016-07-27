// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2016

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
    ProducerConsumer() 
    { 
        stopExecution = false;
    }

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

#ifdef CXX11
    void push(T&& item)
    {
        if (stopExecution == true)
            return;
        boost::unique_lock<boost::mutex> mlock(mutex_);
        queue_.push(std::move(item));
        mlock.unlock();
        cond_.notify_one();
    }
#endif

    void stop()
    {
        boost::unique_lock<boost::mutex> mlock(mutex_);
        stopExecution = true;
        mlock.unlock();
        cond_.notify_one();
        consumerThread.join();
        stopExecution = false;
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
    bool stopExecution;


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
