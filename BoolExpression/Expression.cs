using System;
using System.Text;

namespace ConsoleApp1
{
    class Expression
    {
        public enum BOP { LEAF, AND, OR, NOT };

        private BOP _op;
        private Expression _left;
        private Expression _right;
        private String _lit;

        private Expression(BOP op, Expression left, Expression right)
        {
            _op = op;
            _left = left;
            _right = right;
            _lit = null;
        }

        private Expression(String literal)
        {
            _op = BOP.LEAF;
            _left = null;
            _right = null;
            _lit = literal;
        }

        public BOP Operation { get; set; }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public String Lit { get; set; }

        public static Expression CreateAnd(Expression left, Expression right)
        {
            return new Expression(BOP.AND, left, right);
        }

        public static Expression CreateNot(Expression child)
        {
            return new Expression(BOP.NOT, child, null);
        }

        public static Expression CreateOr(Expression left, Expression right)
        {
            return new Expression(BOP.OR, left, right);
        }

        public static Expression CreateBoolVar(String str)
        {
            return new Expression(str);
        }

        public Expression(Expression other)
        {
            _op = other._op;
            _left = other._left == null ? null : new Expression(other._left);
            _right = other._right == null ? null : new Expression(other._right);
            _lit = new StringBuilder(other._lit).ToString();
        }

        bool IsLeaf()
        {
            return (_op == BOP.LEAF);
        }

        bool IsAtomic()
        {
            return (IsLeaf() || (_op == BOP.NOT && _left.IsLeaf()));
        }
    }
}