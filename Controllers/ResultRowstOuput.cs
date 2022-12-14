namespace projetoFinal.Controllers {
    public class ResultRowstOuput {
        // modelo de retorno para todas as querys exceto get, para get tem que ser expecifica 
       public string ErrorMessage { get; set; }
        public string SucessMessage { get; set; }
        public int RowsAffected { get; set; }
        public ResultRowstOuput() { }
    }
}
