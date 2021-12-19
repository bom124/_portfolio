

#include <iostream>
#include <ios>
#include <vector>
#include <numeric>
#include <algorithm>
#define number_of_win 21
#define number_stop_house 16
#define max_day_honest_month 31
#define max_day_not_honest_month 30
using namespace std;






class Card
{
public:
    friend ostream& operator <<(ostream& os, Card& Card);
    enum enum_suit
    {
        spades = 0,
        hearts,
        clubs,
        diamonds
    };
    enum enum_meaning
    {
        ace = 1,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king
    };
    Card(enum_suit start_suit, enum_meaning start_meaning)
        :suit(start_suit),
        meaning(start_meaning) {}
    void Flip()
    {
        top = !top;
    }
    int GetValue()
    {
        return meaning;
    }
    bool isfaceup()
    {
        return top;
    }
    enum_suit Get_Suit()
    {
        return suit;
    }
private:
    enum_suit suit;
    enum_meaning meaning;
    bool top = true;
};

ostream& operator <<(ostream& os, Card& Card)
{
    const string RANKS[] = { "0", "A", "2", "3", "4", "5", "6", "7", "8", "9","10", "J", "Q", "K" };
    const string SUITS[] = { "s", "h", "c", "d" };

    if (Card.isfaceup())
    {
        os << RANKS[Card.GetValue()];
        os << SUITS[Card.Get_Suit()];
    }
    else
    {
        os << "XX";
    }

    return os;
}

class Hand
{
protected:
    vector<Card*> card;
public:
    void Add(Card* new_card)
    {
        card.push_back(new_card);
    }
    void Clear()
    {
        card.clear();
    }
    int GetTotal() const
    {
        int summ = 0;
        for (int i = 0; i < card.size(); i++)
        {
            summ += card[i]->GetValue();
        }
        return summ;
    }
};

class GenericPlayer : public Hand
{
public:
    friend ostream& operator <<(ostream& os, const GenericPlayer& aGenericPlayer);
    virtual bool IsHitting() const = 0;
    bool IsBoosted()
    {
        return GetTotal() > number_of_win ? true : false;
    }
    void Bust()
    {
        cout << name << "has a bust of cards" << endl;
    }
protected:
    string name;
};


ostream& operator<<(ostream& os, const GenericPlayer& aGenericPlayer)
{
    os << aGenericPlayer.name << ":\t";
    vector<Card*>::const_iterator pCard;
    if (!aGenericPlayer.card.empty())
    {
        for (pCard = aGenericPlayer.card.begin();
            pCard != aGenericPlayer.card.end();
            ++pCard)
        {
            os << *(*pCard) << "\t";
        }


        if (aGenericPlayer.GetTotal() != 0)
        {
            cout << "(" << aGenericPlayer.GetTotal() << ")";
        }
    }
    else
    {
        os << "<empty>";
    }

    return os;
}


class Player : public GenericPlayer
{
public:
    Player(const string& start_name = "None name") { name = start_name; };

    virtual ~Player() {};


    virtual bool IsHitting() const
    {
        cout << name << ", do you want a hit? (Y/N): ";
        char response;
        cin >> response;
        return (response == 'y' || response == 'Y');
    }


    void Win() const
    {
        cout << name << " wins" << endl;
    }


    void Lose() const
    {
        cout << name << " loses" << endl;
    }



    void Push() const
    {
        cout << name << " pushes" << endl;
    }

};

class House : public GenericPlayer
{
public:
    House(const string& start_name = "House") { name = start_name; };
    virtual ~House() {};


    virtual bool IsHitting() const
    {
        return (GetTotal() <= number_stop_house);
    }



    void FlipFirstCard()
    {
        if (!(card.empty()))
        {
            card[0]->Flip();
        }
        else
        {
            cout << "No card to flip!" << endl;
        }
    }

};

class Deck : public Hand
{
public:
    Deck() { Populate(); };

    virtual ~Deck() {};

    void Populate()
    {
        Clear();
        for (int s = Card::spades; s <= Card::diamonds; ++s)
        {
            for (int r = Card::ace; r <= Card::king; ++r)
            {
                Add(new Card(Card::enum_suit(s), Card::enum_meaning(r)));
            }
        }
    };

    void Shuffle()
    {
        random_shuffle(card.begin(), card.end());
    };

    void Deal(Hand& aHand)
    {
        if (!card.empty())
        {
            aHand.Add(card.back());
            card.pop_back();
        }
        else
        {
            cout << "Out of cards. Unable to deal.";
        }

    }

    void AdditionalCards(GenericPlayer& aGenericPlayer)
    {
        cout << endl;
        while (!(aGenericPlayer.IsBoosted()) && aGenericPlayer.IsHitting())
        {
            Deal(aGenericPlayer);
            cout << aGenericPlayer << endl;

            if (aGenericPlayer.IsBoosted())
            {
                aGenericPlayer.Bust();
            }
        }

    };
};

class Game
{
public:
    Game(const vector<string>& names)
    {

        vector<string>::const_iterator pName;
        for (pName = names.begin(); pName != names.end(); ++pName)
        {
            players.push_back(Player(*pName));
        }


        srand(static_cast<unsigned int>(time(0)));
        deck.Populate();
        deck.Shuffle();
    }

    ~Game()
    {};

    void Play()
    {

        vector<Player>::iterator pPlayer;
        for (int i = 0; i < 2; ++i)
        {
            for (pPlayer = players.begin(); pPlayer != players.end(); ++pPlayer)
            {
                deck.Deal(*pPlayer);
            }
            deck.Deal(house);
        }


        house.FlipFirstCard();


        for (pPlayer = players.begin(); pPlayer != players.end(); ++pPlayer)
        {
            cout << *pPlayer << endl;
        }
        cout << house << endl;


        for (pPlayer = players.begin(); pPlayer != players.end(); ++pPlayer)
        {
            deck.AdditionalCards(*pPlayer);
        }


        house.FlipFirstCard();
        cout << endl << house;


        deck.AdditionalCards(house);

        if (house.IsBoosted())
        {

            for (pPlayer = players.begin(); pPlayer != players.end(); ++pPlayer)
            {
                if (!(pPlayer->IsBoosted()))
                {
                    pPlayer->Win();
                }
            }
        }
        else
        {

            for (pPlayer = players.begin(); pPlayer != players.end();
                ++pPlayer)
            {
                if (!(pPlayer->IsBoosted()))
                {
                    if (pPlayer->GetTotal() > house.GetTotal())
                    {
                        pPlayer->Win();
                    }
                    else if (pPlayer->GetTotal() < house.GetTotal())
                    {
                        pPlayer->Lose();
                    }
                    else
                    {
                        pPlayer->Push();
                    }
                }
            }

        }


        for (pPlayer = players.begin(); pPlayer != players.end(); ++pPlayer)
        {
            pPlayer->Clear();
        }
        house.Clear();
    }

private:
    Deck deck;
    House house;
    vector<Player> players;
};





int main()
{
    vector<string> vectorstr = { "Jeff" , "Djon","Sack" };
    Game game(vectorstr);
    game.Play();
}